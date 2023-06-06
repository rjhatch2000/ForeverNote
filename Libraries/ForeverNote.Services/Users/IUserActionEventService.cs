using ForeverNote.Core.Domain.Users;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    public partial interface IUserActionEventService
    {
        /// <summary>
        /// Viewed
        /// </summary>
        Task Viewed(User user, string currentUrl, string previousUrl);

        /// <summary>
        /// Run action url
        /// </summary>
        Task Url(User user, string currentUrl, string previousUrl);

        /// <summary>
        /// Run action url
        /// </summary>
        Task Registration(User user);
    }
}
