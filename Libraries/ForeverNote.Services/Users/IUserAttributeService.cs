using ForeverNote.Core.Domain.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    /// <summary>
    /// User attribute service
    /// </summary>
    public partial interface IUserAttributeService
    {
        /// <summary>
        /// Deletes a user attribute
        /// </summary>
        /// <param name="userAttribute">User attribute</param>
        Task DeleteUserAttribute(UserAttribute userAttribute);

        /// <summary>
        /// Gets all user attributes
        /// </summary>
        /// <returns>User attributes</returns>
        Task<IList<UserAttribute>> GetAllUserAttributes();

        /// <summary>
        /// Gets a user attribute 
        /// </summary>
        /// <param name="userAttributeId">User attribute identifier</param>
        /// <returns>User attribute</returns>
        Task<UserAttribute> GetUserAttributeById(string userAttributeId);

        /// <summary>
        /// Inserts a user attribute
        /// </summary>
        /// <param name="userAttribute">User attribute</param>
        Task InsertUserAttribute(UserAttribute userAttribute);

        /// <summary>
        /// Updates the user attribute
        /// </summary>
        /// <param name="userAttribute">User attribute</param>
        Task UpdateUserAttribute(UserAttribute userAttribute);

        /// <summary>
        /// Deletes a user attribute value
        /// </summary>
        /// <param name="userAttributeValue">User attribute value</param>
        Task DeleteUserAttributeValue(UserAttributeValue userAttributeValue);

        /// <summary>
        /// Inserts a user attribute value
        /// </summary>
        /// <param name="userAttributeValue">User attribute value</param>
        Task InsertUserAttributeValue(UserAttributeValue userAttributeValue);

        /// <summary>
        /// Updates the user attribute value
        /// </summary>
        /// <param name="userAttributeValue">User attribute value</param>
        Task UpdateUserAttributeValue(UserAttributeValue userAttributeValue);
    }
}
