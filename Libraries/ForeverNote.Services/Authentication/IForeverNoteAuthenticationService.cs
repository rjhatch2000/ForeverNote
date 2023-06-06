using ForeverNote.Core.Domain.Users;
using System.Threading.Tasks;

namespace ForeverNote.Services.Authentication
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public partial interface IForeverNoteAuthenticationService
    {
        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="createPersistentCookie">A value indicating whether to create a persistent cookie</param>
        Task SignIn(User user, bool createPersistentCookie);

        /// <summary>
        /// Sign out
        /// </summary>
        Task SignOut();

        /// <summary>
        /// Get authenticated user
        /// </summary>
        /// <returns>User</returns>
        Task<User> GetAuthenticatedUser();
    }
}