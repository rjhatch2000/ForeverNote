using ForeverNote.Core;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Services.Common;
using ForeverNote.Services.Events.Web;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Security;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    /// <summary>
    /// User registration service
    /// </summary>
    public partial class UrerRegistrationService : IUserRegistrationService
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IEncryptionService _encryptionService;
        private readonly ILocalizationService _localizationService;
        private readonly IMediator _mediator;
        private readonly UserSettings _userSettings;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="userService">User service</param>
        /// <param name="encryptionService">Encryption service</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="mediator">Mediator</param>
        /// <param name="userSettings">User settings</param>
        public UrerRegistrationService(IUserService userService, 
            IEncryptionService encryptionService, 
            ILocalizationService localizationService,
            IMediator mediator,
            UserSettings userSettings
        )
        {
            _userService = userService;
            _encryptionService = encryptionService;
            _localizationService = localizationService;
            _mediator = mediator;
            _userSettings = userSettings;
        }

        #endregion

        #region Methods

        protected bool PasswordMatch(UserHistoryPassword userPassword, ChangePasswordRequest request)
        {
            string newPwd = "";
            switch (request.NewPasswordFormat)
            {
                case PasswordFormat.Encrypted:
                    newPwd = _encryptionService.EncryptText(request.NewPassword);
                    break;
                case PasswordFormat.Hashed:
                    newPwd = _encryptionService.CreatePasswordHash(request.NewPassword, userPassword.PasswordSalt, _userSettings.HashedPasswordFormat);
                    break;
                default:
                    newPwd = request.NewPassword;
                    break;
            }

            return userPassword.Password.Equals(newPwd);
        }


        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="usernameOrEmail">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        public virtual async Task<UserLoginResults> ValidateUser(string usernameOrEmail, string password)
        {
            var user = _userSettings.UsernamesEnabled ?
                await _userService.GetUserByUsername(usernameOrEmail) :
                await _userService.GetUserByEmail(usernameOrEmail);

            if (user == null)
                return UserLoginResults.UserNotExist;
            if (user.Deleted)
                return UserLoginResults.Deleted;
            if (!user.Active)
                return UserLoginResults.NotActive;

            //TODO: This needs to be figured out, yes?
            //only registered can login
            //if (!user.IsRegistered())
            //    return UserLoginResults.NotRegistered;

            if (user.CannotLoginUntilDateUtc.HasValue && user.CannotLoginUntilDateUtc.Value > DateTime.UtcNow)
                return UserLoginResults.LockedOut;

            string pwd = "";
            switch (user.PasswordFormat)
            {
                case PasswordFormat.Encrypted:
                    pwd = _encryptionService.EncryptText(password);
                    break;
                case PasswordFormat.Hashed:
                    pwd = _encryptionService.CreatePasswordHash(password, user.PasswordSalt, _userSettings.HashedPasswordFormat);
                    break;
                default:
                    pwd = password;
                    break;
            }

            bool isValid = pwd == user.Password;
            if (!isValid)
            {
                //wrong password
                user.FailedLoginAttempts++;
                if (_userSettings.FailedPasswordAllowedAttempts > 0 &&
                    user.FailedLoginAttempts >= _userSettings.FailedPasswordAllowedAttempts)
                {
                    //lock out
                    user.CannotLoginUntilDateUtc = DateTime.UtcNow.AddMinutes(_userSettings.FailedPasswordLockoutMinutes);
                    //reset the counter
                    user.FailedLoginAttempts = 0;
                }
                await _userService.UpdateUserLastLoginDate(user);
                return UserLoginResults.WrongPassword;
            }

            //2fa required
            if (user.GetAttributeFromEntity<bool>(SystemUserAttributeNames.TwoFactorEnabled) && _userSettings.TwoFactorAuthenticationEnabled)
                return UserLoginResults.RequiresTwoFactor;
            
            //save last login date
            user.FailedLoginAttempts = 0;
            user.CannotLoginUntilDateUtc = null;
            user.LastLoginDateUtc = DateTime.UtcNow;
            await _userService.UpdateUserLastLoginDate(user);
            return UserLoginResults.Successful;
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual async Task<UserRegistrationResult> RegisterUser(UserRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.User == null)
                throw new ArgumentException("Can't load current user");

            var result = new UserRegistrationResult();
            if (request.User.IsBackgroundTaskAccount())
            {
                result.AddError("Background task account can't be registered");
                return result;
            }
            if (String.IsNullOrEmpty(request.Email))
            {
                result.AddError(_localizationService.GetResource("Account.Register.Errors.EmailIsNotProvided"));
                return result;
            }
            if (!CommonHelper.IsValidEmail(request.Email))
            {
                result.AddError(_localizationService.GetResource("Common.WrongEmail"));
                return result;
            }
            if (String.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError(_localizationService.GetResource("Account.Register.Errors.PasswordIsNotProvided"));
                return result;
            }
            if (_userSettings.UsernamesEnabled)
            {
                if (String.IsNullOrEmpty(request.Username))
                {
                    result.AddError(_localizationService.GetResource("Account.Register.Errors.UsernameIsNotProvided"));
                    return result;
                }
            }

            //validate unique user
            if (await _userService.GetUserByEmail(request.Email) != null)
            {
                result.AddError(_localizationService.GetResource("Account.Register.Errors.EmailAlreadyExists"));
                return result;
            }
            if (_userSettings.UsernamesEnabled)
            {
                if (await _userService.GetUserByUsername(request.Username) != null)
                {
                    result.AddError(_localizationService.GetResource("Account.Register.Errors.UsernameAlreadyExists"));
                    return result;
                }
            }

            //event notification
            await _mediator.UserRegistrationEvent(result, request);

            //return if exist errors
            if (result.Errors.Any())
                return result;

            //at this point request is valid
            request.User.Username = request.Username;
            request.User.Email = request.Email;
            request.User.PasswordFormat = request.PasswordFormat;

            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    {
                        request.User.Password = request.Password;
                    }
                    break;
                case PasswordFormat.Encrypted:
                    {
                        request.User.Password = _encryptionService.EncryptText(request.Password);
                    }
                    break;
                case PasswordFormat.Hashed:
                    {
                        string saltKey = _encryptionService.CreateSaltKey(5);
                        request.User.PasswordSalt = saltKey;
                        request.User.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey, _userSettings.HashedPasswordFormat);
                    }
                    break;
                default:
                    break;
            }

            await _userService.InsertUserPassword(request.User);

            request.User.Active = request.IsApproved;
            await _userService.UpdateActive(request.User);
            request.User.PasswordChangeDateUtc = DateTime.UtcNow;
            await _userService.UpdateUser(request.User);

            return result;
        }
        
        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual async Task<ChangePasswordResult> ChangePassword(ChangePasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var result = new ChangePasswordResult();
            if (String.IsNullOrWhiteSpace(request.Email))
            {
                result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.EmailIsNotProvided"));
                return result;
            }
            if (String.IsNullOrWhiteSpace(request.NewPassword))
            {
                result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.PasswordIsNotProvided"));
                return result;
            }

            var user = await _userService.GetUserByEmail(request.Email);
            if (user == null)
            {
                result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.EmailNotFound"));
                return result;
            }

            if (request.ValidateRequest)
            {
                string oldPwd = "";
                switch (user.PasswordFormat)
                {
                    case PasswordFormat.Encrypted:
                        oldPwd = _encryptionService.EncryptText(request.OldPassword);
                        break;
                    case PasswordFormat.Hashed:
                        oldPwd = _encryptionService.CreatePasswordHash(request.OldPassword, user.PasswordSalt, _userSettings.HashedPasswordFormat);
                        break;
                    default:
                        oldPwd = request.OldPassword;
                        break;
                }

                if (oldPwd != user.Password)
                {
                    result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.OldPasswordDoesntMatch"));
                    return result;
                }
            }

            //check for duplicates
            if (_userSettings.UnduplicatedPasswordsNumber > 0)
            {
                //get some of previous passwords
                var previousPasswords = await _userService.GetPasswords(user.Id, passwordsToReturn: _userSettings.UnduplicatedPasswordsNumber);

                var newPasswordMatchesWithPrevious = previousPasswords.Any(password => PasswordMatch(password, request));
                if (newPasswordMatchesWithPrevious)
                {
                    result.AddError(_localizationService.GetResource("Account.ChangePassword.Errors.PasswordMatchesWithPrevious"));
                    return result;
                }
            }

            switch (request.NewPasswordFormat)
            {
                case PasswordFormat.Clear:
                    {
                        user.Password = request.NewPassword;
                    }
                    break;
                case PasswordFormat.Encrypted:
                    {
                        user.Password = _encryptionService.EncryptText(request.NewPassword);
                    }
                    break;
                case PasswordFormat.Hashed:
                    {
                        string saltKey = _encryptionService.CreateSaltKey(5);
                        user.PasswordSalt = saltKey;
                        user.Password = _encryptionService.CreatePasswordHash(request.NewPassword, saltKey, _userSettings.HashedPasswordFormat);
                    }
                    break;
                default:
                    break;
            }
            user.PasswordChangeDateUtc = DateTime.UtcNow;
            user.PasswordFormat = request.NewPasswordFormat;
            await _userService.UpdateUser(user);
            await _userService.InsertUserPassword(user);

            return result;
        }

        /// <summary>
        /// Sets a user email
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newEmail">New email</param>
        public virtual async Task SetEmail(User user, string newEmail)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (newEmail == null)
                throw new ForeverNoteException("Email cannot be null");

            newEmail = newEmail.Trim();
            string oldEmail = user.Email;

            if (!CommonHelper.IsValidEmail(newEmail))
                throw new ForeverNoteException(_localizationService.GetResource("Account.EmailUsernameErrors.NewEmailIsNotValid"));

            if (newEmail.Length > 100)
                throw new ForeverNoteException(_localizationService.GetResource("Account.EmailUsernameErrors.EmailTooLong"));

            var user2 = await _userService.GetUserByEmail(newEmail);
            if (user2 != null && user.Id != user2.Id)
                throw new ForeverNoteException(_localizationService.GetResource("Account.EmailUsernameErrors.EmailAlreadyExists"));

            user.Email = newEmail;
            await _userService.UpdateUser(user);
        }

        /// <summary>
        /// Sets a user username
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newUsername">New Username</param>
        public virtual async Task SetUsername(User user, string newUsername)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (!_userSettings.UsernamesEnabled)
                throw new ForeverNoteException("Usernames are disabled");

            if (!_userSettings.AllowUsersToChangeUsernames)
                throw new ForeverNoteException("Changing usernames is not allowed");

            newUsername = newUsername.Trim();

            if (newUsername.Length > 100)
                throw new ForeverNoteException(_localizationService.GetResource("Account.EmailUsernameErrors.UsernameTooLong"));

            var user2 = await _userService.GetUserByUsername(newUsername);
            if (user2 != null && user.Id != user2.Id)
                throw new ForeverNoteException(_localizationService.GetResource("Account.EmailUsernameErrors.UsernameAlreadyExists"));

            user.Username = newUsername;
            await _userService.UpdateUser(user);
        }

        #endregion
    }
}