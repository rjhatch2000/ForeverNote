using ForeverNote.Core;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    /// <summary>
    /// User manager service
    /// </summary>
    public class UserManagerService : IUserManagerService
    {
        #region Fields

        private readonly IUserService _userService;
        ////private readonly IGroupService _groupService;
        private readonly IEncryptionService _encryptionService;
        private readonly ITranslationService _translationService;
        private readonly IMediator _mediator;
        ////private readonly IGenericAttributeService _genericAttributeService;
        private readonly IUserHistoryPasswordService _userHistoryPasswordService;
        private readonly UserSettings _userSettings;
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="userService">User service</param>
        /// <param name="groupService">Group service</param>
        /// <param name="encryptionService">Encryption service</param>
        /// <param name="translationService">Translation service</param>
        /// <param name="mediator">Mediator</param>
        /// <param name="genericAttributeService">GenericAttributes service</param>
        /// <param name="userHistoryPasswordService">History password</param>
        /// <param name="userSettings">User settings</param>
        public UserManagerService(
            IUserService userService,
            ////IGroupService groupService,
            IEncryptionService encryptionService,
            ITranslationService translationService,
            IMediator mediator,
            ////IGenericAttributeService genericAttributeService,
            IUserHistoryPasswordService userHistoryPasswordService,
            UserSettings userSettings)
        {
            _userService = userService;
            ////_groupService = groupService;
            _encryptionService = encryptionService;
            _translationService = translationService;
            _mediator = mediator;
            ////_genericAttributeService = genericAttributeService;
            _userHistoryPasswordService = userHistoryPasswordService;
            _userSettings = userSettings;
        }

        #endregion

        #region Methods

        public virtual bool PasswordMatch(PasswordFormat passwordFormat, string oldPassword, string newPassword, string passwordSalt)
        {
            var newPwd = passwordFormat switch
            {
                PasswordFormat.Clear => newPassword,
                PasswordFormat.Encrypted => _encryptionService.EncryptText(newPassword, passwordSalt),
                PasswordFormat.Hashed => _encryptionService.CreatePasswordHash(newPassword, passwordSalt, _userSettings.HashedPasswordFormat),
                _ => throw new Exception("PasswordFormat not supported")
            };
            return oldPassword.Equals(newPwd);
        }


        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="usernameOrEmail">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        public virtual async Task<UserLoginResults> LoginUser(string usernameOrEmail, string password)
        {
            var user = _userSettings.UsernamesEnabled ? await _userService.GetUserByUsername(usernameOrEmail) : await _userService.GetUserByEmail(usernameOrEmail);

            var pwd = user.PasswordFormatId switch
            {
                (int)PasswordFormat.Clear => password,
                (int)PasswordFormat.Encrypted => _encryptionService.EncryptText(password, user.PasswordSalt),
                (int)PasswordFormat.Hashed => _encryptionService.CreatePasswordHash(password, user.PasswordSalt, _userSettings.HashedPasswordFormat),
                _ => throw new Exception("PasswordFormat not supported")
            };
            var isValid = pwd == user.Password;
            if (!isValid)
                return UserLoginResults.WrongPassword;

            //2fa required
            if (user.TwoFactorEnabled && _userSettings.TwoFactorAuthenticationEnabled)
                return UserLoginResults.RequiresTwoFactor;

            return UserLoginResults.Successful;
        }

        /////// <summary>
        /////// Register user
        /////// </summary>
        /////// <param name="request">Request</param>
        /////// <returns>Result</returns>
        ////public virtual async Task RegisterUser(RegistrationRequest request)
        ////{
        ////    if (request == null)
        ////        throw new ArgumentNullException(nameof(request));

        ////    if (request.User == null)
        ////        throw new ArgumentException("Can't load current user");

        ////    //event notification
        ////    await _mediator.UserRegistrationEvent(request);

        ////    request.User.Username = request.Username;
        ////    request.User.Email = request.Email;
        ////    request.User.PasswordFormatId = request.PasswordFormat;
        ////    request.User.StoreId = request.StoreId;

        ////    switch (request.PasswordFormat)
        ////    {
        ////        case PasswordFormat.Clear:
        ////            request.User.Password = request.Password;
        ////            break;
        ////        case PasswordFormat.Encrypted:
        ////            request.User.PasswordSalt = CommonHelper.GenerateRandomDigitCode(24);
        ////            request.User.Password = _encryptionService.EncryptText(request.Password, request.User.PasswordSalt);
        ////            break;
        ////        case PasswordFormat.Hashed:
        ////            var saltKey = _encryptionService.CreateSaltKey(5);
        ////            request.User.PasswordSalt = saltKey;
        ////            request.User.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey, _userSettings.HashedPasswordFormat);
        ////            break;
        ////        default:
        ////            break;
        ////    }
        ////    await _userHistoryPasswordService.InsertUserPassword(request.User);

        ////    request.User.Active = request.IsApproved;
        ////    await _userService.UpdateActive(request.User);
        ////    //add to 'Registered' role
        ////    var registeredRole = await _groupService.GetUserGroupBySystemName(SystemUserGroupNames.Registered);
        ////    if (registeredRole == null)
        ////        throw new GrandException("'Registered' role could not be loaded");
        ////    request.User.Groups.Add(registeredRole.Id);
        ////    await _userService.InsertUserGroupInUser(registeredRole, request.User.Id);
        ////    //remove from 'Guests' role
        ////    var guestGroup = await _groupService.GetUserGroupBySystemName(SystemUserGroupNames.Guests);
        ////    var guestExists = request.User.Groups.FirstOrDefault(cr => cr == guestGroup?.Id);
        ////    if (guestExists != null)
        ////    {
        ////        request.User.Groups.Remove(guestGroup.Id);
        ////        await _userService.DeleteUserGroupInUser(guestGroup, request.User.Id);
        ////    }
        ////    request.User.PasswordChangeDateUtc = DateTime.UtcNow;
        ////    await _userService.UpdateUser(request.User);

        ////}

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        public virtual async Task ChangePassword(ChangePasswordRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var user = await _userService.GetUserByEmail(request.Email);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    {
                        user.Password = request.NewPassword;
                    }
                    break;
                case PasswordFormat.Encrypted:
                    {
                        user.PasswordSalt = CommonHelper.GenerateRandomDigitCode(24);
                        user.Password = _encryptionService.EncryptText(request.NewPassword, user.PasswordSalt);
                    }
                    break;
                case PasswordFormat.Hashed:
                    {
                        var saltKey = _encryptionService.CreateSaltKey(5);
                        user.PasswordSalt = saltKey;
                        user.Password = _encryptionService.CreatePasswordHash(request.NewPassword, saltKey, _userSettings.HashedPasswordFormat);
                    }
                    break;
                default:
                    break;
            }
            user.PasswordChangeDateUtc = DateTime.UtcNow;
            user.PasswordFormatId = (int)request.PasswordFormat;
            user.PasswordToken = Guid.NewGuid().ToString();
            await _userService.UpdateUser(user);
            //insert password history
            await _userHistoryPasswordService.InsertUserPassword(user);

            //////create new login token
            ////await user.PasswordToken _genericAttributeService.SaveField(user, SystemGenericAttributeNames.PasswordToken, Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Sets a user email
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newEmail">New email</param>
        public virtual async Task SetEmail(User user, string newEmail)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (newEmail == null)
                throw new ForeverNoteException("Email cannot be null");

            newEmail = newEmail.Trim();

            if (!CommonHelper.IsValidEmail(newEmail))
                throw new ForeverNoteException(_translationService.GetResource("Account.EmailUsernameErrors.NewEmailIsNotValid"));

            if (newEmail.Length > 100)
                throw new ForeverNoteException(_translationService.GetResource("Account.EmailUsernameErrors.EmailTooLong"));

            var user2 = await _userService.GetUserByEmail(newEmail);
            if (user2 != null && user.Id != user2.Id)
                throw new ForeverNoteException(_translationService.GetResource("Account.EmailUsernameErrors.EmailAlreadyExists"));

            user.Email = newEmail;
            await _userService.UpdateUser(user);

            //update newsletter subscription (if required)
            //TODO
            /*
            if (!String.IsNullOrEmpty(oldEmail) && !oldEmail.Equals(newEmail, StringComparison.OrdinalIgnoreCase))
            {
                foreach (var store in await _storeService.GetAllStores())
                {
                    var subscriptionOld = await _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(oldEmail, store.Id);
                    if (subscriptionOld != null)
                    {
                        subscriptionOld.Email = newEmail;
                        await _newsLetterSubscriptionService.UpdateNewsLetterSubscription(subscriptionOld);
                    }
                }
            }*/
        }

        /// <summary>
        /// Sets a user username
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newUsername">New Username</param>
        public virtual async Task SetUsername(User user, string newUsername)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!_userSettings.UsernamesEnabled)
                throw new ForeverNoteException("Usernames are disabled");

            if (!_userSettings.AllowUsersToChangeUsernames)
                throw new ForeverNoteException("Changing usernames is not allowed");

            newUsername = newUsername.Trim();

            if (newUsername.Length > 100)
                throw new ForeverNoteException(_translationService.GetResource("Account.EmailUsernameErrors.UsernameTooLong"));

            var user2 = await _userService.GetUserByUsername(newUsername);
            if (user2 != null && user.Id != user2.Id)
                throw new ForeverNoteException(_translationService.GetResource("Account.EmailUsernameErrors.UsernameAlreadyExists"));

            user.Username = newUsername;
            await _userService.UpdateUser(user);
        }

        #endregion
    }
}
