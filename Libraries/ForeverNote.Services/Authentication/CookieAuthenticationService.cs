using ForeverNote.Core;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Services.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ForeverNote.Services.Authentication
{
    /// <summary>
    /// Represents service using cookie middleware for the authentication
    /// </summary>
    public partial class CookieAuthenticationService : IForeverNoteAuthenticationService
    {
        #region Fields

        private readonly UserSettings _userSettings;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private User _cachedUser;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="userSettings">User settings</param>
        /// <param name="userService">User service</param>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        public CookieAuthenticationService(UserSettings userSettings,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userSettings = userSettings;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="isPersistent">Whether the authentication session is persisted across multiple requests</param>
        public virtual async Task SignIn(User user, bool isPersistent)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //create claims for user's username and email
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(user.Username))
                claims.Add(new Claim(ClaimTypes.Name, user.Username, ClaimValueTypes.String, ForeverNoteCookieAuthenticationDefaults.ClaimsIssuer));

            if (!string.IsNullOrEmpty(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email, ForeverNoteCookieAuthenticationDefaults.ClaimsIssuer));

            //create principal for the current authentication scheme
            var userIdentity = new ClaimsIdentity(claims, ForeverNoteCookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            //set value indicating whether session is persisted and the time at which the authentication was issued
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddHours(CommonHelper.CookieAuthExpires)
            };

            //sign in
            await _httpContextAccessor.HttpContext.SignInAsync(ForeverNoteCookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);

            //cache authenticated user
            _cachedUser = user;
        }

        /// <summary>
        /// Sign out
        /// </summary>
        public virtual async Task SignOut()
        {
            //reset cached user
            _cachedUser = null;

            //and sign out from the current authentication scheme
            await _httpContextAccessor.HttpContext.SignOutAsync(ForeverNoteCookieAuthenticationDefaults.AuthenticationScheme);
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

            //try to get authenticated user identity
            var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(ForeverNoteCookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return null;

            User user = null;
            if (_userSettings.UsernamesEnabled)
            {
                //try to get user by username
                var usernameClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Name
                    && claim.Issuer.Equals(ForeverNoteCookieAuthenticationDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
                if (usernameClaim != null)
                    user = await _userService.GetUserByUsername(usernameClaim.Value);
            }
            else
            {
                //try to get user by email
                var emailClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email
                    && claim.Issuer.Equals(ForeverNoteCookieAuthenticationDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
                if (emailClaim != null)
                    user = await _userService.GetUserByEmail(emailClaim.Value);
            }

            //whether the found user is available
            if (user == null || !user.Active || user.Deleted)
                return null;

            //cache authenticated user
            _cachedUser = user;

            return _cachedUser;
        }

        #endregion
    }
}