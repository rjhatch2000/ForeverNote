using ForeverNote.Core.Domain.Users;
using ForeverNote.Services.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Authentication
{
    public partial class ApiAuthenticationService : IApiAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IUserApiService _userApiService;

        private User _cachedUser;

        private string _errorMessage;
        private string _email;

        public ApiAuthenticationService(IHttpContextAccessor httpContextAccessor,
            IUserService userService, IUserApiService userApiService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _userApiService = userApiService;
        }

        /// <summary>
        /// Valid
        /// </summary>
        /// <param name="user">User</param>
        public virtual async Task<bool> Valid(TokenValidatedContext context)
        {
            _email = context.Principal.Claims.ToList().FirstOrDefault(x => x.Type == "Email")?.Value;

            if (string.IsNullOrEmpty(_email))
            {
                _errorMessage = "Email not exists in the context";
                return await Task.FromResult(false);
            }
            var user = await _userService.GetUserByEmail(_email);
            if (user == null || !user.Active || user.Deleted)
            {
                _errorMessage = "Email not exists/or not active in the user table";
                return await Task.FromResult(false);
            }
            var userapi = await _userApiService.GetUserByEmail(_email);
            if (userapi == null || !userapi.IsActive)
            {
                _errorMessage = "User api not exists/or not active in the user api table";
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }

        public virtual async Task SignIn()
        {
            if (string.IsNullOrEmpty(_email))
                throw new ArgumentNullException(nameof(_email));

            await SignIn(_email);
        }

        /// <summary>
        /// Sign in
        /// </summary>
        ///<param name="email">Email</param>
        public virtual async Task SignIn(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));

            var user = await _userService.GetUserByEmail(email);
            if (user != null)
                _cachedUser = user;
        }


        /// <summary>
        /// Get error message
        /// </summary>
        /// <returns></returns>
        public virtual Task<string> ErrorMessage()
        {
            return Task.FromResult(_errorMessage);
        }

        /// <summary>
        /// Get authenticated user
        /// </summary>
        /// <returns>User</returns>
        public virtual async Task<User> GetAuthenticatedUser()
        {
            //whether there is a cached user
            if (_cachedUser != null)
                return _cachedUser;

            User user = null;

            if (_httpContextAccessor.HttpContext.Request.Path.Value.ToLowerInvariant().Contains("/api/token/create"))
            {
                user = await _userService.GetUserBySystemName(SystemUserNames.BackgroundTask);
                if (user != null)
                    return user;
            }

            //try to get authenticated user identity
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith(JwtBearerDefaults.AuthenticationScheme))
                return null;

            var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return null;

            //try to get user by email
            var emailClaim = authenticateResult.Principal.Claims.FirstOrDefault(claim => claim.Type == "Email");
            if (emailClaim != null)
                user = await _userService.GetUserByEmail(emailClaim.Value);


            //whether the found user is available
            if (user == null || !user.Active || user.Deleted)
                return null;

            //cache authenticated user
            _cachedUser = user;

            return _cachedUser;

        }

    }
}
