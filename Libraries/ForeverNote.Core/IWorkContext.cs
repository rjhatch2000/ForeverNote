using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Localization;
using System.Threading.Tasks;

namespace ForeverNote.Core
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        User CurrentUser { get; set; }

        /// <summary>
        /// Set the current user by Middleware
        /// </summary>
        /// <returns></returns>
        Task<User> SetCurrentUser();

        /// <summary>
        /// Gets or sets the original user (in case the current one is impersonated)
        /// </summary>
        User OriginalUserIfImpersonated { get; }

        /// <summary>
        /// Get or set current user working language
        /// </summary>
        Language WorkingLanguage { get; }

        /// <summary>
        /// Set current user working language by Middleware
        /// </summary>
        Task<Language> SetWorkingLanguage(User user);

        /// <summary>
        /// Set current user working language
        /// </summary>
        Task<Language> SetWorkingLanguage(Language language);

    }
}
