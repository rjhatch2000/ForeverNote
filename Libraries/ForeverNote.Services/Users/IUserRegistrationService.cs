using ForeverNote.Core.Domain.Users;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    /// <summary>
    /// User registration interface
    /// </summary>
    public partial interface IUserRegistrationService
    {
        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="usernameOrEmail">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        Task<UserLoginResults> ValidateUser(string usernameOrEmail, string password);

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        Task<UserRegistrationResult> RegisterUser(UserRegistrationRequest request);

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        Task<ChangePasswordResult> ChangePassword(ChangePasswordRequest request);

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