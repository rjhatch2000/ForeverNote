using ForeverNote.Core.Domain.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;

namespace ForeverNote.Services.Authentication
{
    public partial interface IApiAuthenticationService
    {
        /// <summary>
        /// Sign in
        /// </summary>
        Task SignIn();

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="email">email</param>
        Task SignIn(string email);

        /// <summary>
        /// Valid email 
        /// </summary>
        ///<param name="context">Token</param>
        Task<bool> Valid(TokenValidatedContext context);

        /// <summary>
        /// Get error message
        /// </summary>
        /// <returns></returns>
        Task<string> ErrorMessage();

        /// <summary>
        /// Get authenticated user
        /// </summary>
        /// <returns>User</returns>
        Task<User> GetAuthenticatedUser();
    }
}
