using ForeverNote.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    /// <summary>
    /// User manager interface
    /// </summary>
    public interface IUserManagerService
    {
        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="usernameOrEmail">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        Task<UserLoginResults> LoginUser(string usernameOrEmail, string password);

        ///// <summary>
        ///// Register user
        ///// </summary>
        ///// <param name="request">Request</param>
        //Task RegisterUser(RegistrationRequest request);

        /// <summary>
        /// Password match
        /// </summary>
        /// <param name="passwordFormat"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        bool PasswordMatch(PasswordFormat passwordFormat, string oldPassword, string newPassword, string passwordSalt);

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        Task ChangePassword(ChangePasswordRequest request);

        /// <summary>
        /// Sets a user email
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newEmail">New email</param>
        Task SetEmail(User user, string newEmail);

        /// <summary>
        /// Sets a user username
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="newUsername">New Username</param>
        Task SetUsername(User user, string newUsername);
    }
}
