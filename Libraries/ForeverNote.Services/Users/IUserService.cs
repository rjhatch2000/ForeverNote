using ForeverNote.Core;
using ForeverNote.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    /// <summary>
    /// User service interface
    /// </summary>
    public partial interface IUserService
    {
        #region Users

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="email">Email; null to load all users</param>
        /// <param name="username">Username; null to load all users</param>
        /// <param name="firstName">First name; null to load all users</param>
        /// <param name="lastName">Last name; null to load all users</param>
        /// <param name="dayOfBirth">Day of birth; 0 to load all users</param>
        /// <param name="monthOfBirth">Month of birth; 0 to load all users</param>
        /// <param name="company">Company; null to load all users</param>
        /// <param name="phone">Phone; null to load all users</param>
        /// <param name="zipPostalCode">Phone; null to load all users</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Users</returns>
        Task<IPagedList<User>> GetAllUsers(DateTime? createdFromUtc = null,
            DateTime? createdToUtc = null,
            string[] userTagIds = null, string email = null, string username = null,
            string firstName = null, string lastName = null,
            string company = null, string phone = null, string zipPostalCode = null,
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets all users by user format (including deleted ones)
        /// </summary>
        /// <param name="passwordFormat">Password format</param>
        /// <returns>Users</returns>
        Task<IList<User>> GetAllUsersByPasswordFormat(PasswordFormat passwordFormat);

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user">User</param>
        Task DeleteUser(User user);

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>A user</returns>
        Task<User> GetUserById(string userId);

        /// <summary>
        /// Get users by identifiers
        /// </summary>
        /// <param name="userIds">User identifiers</param>
        /// <returns>Users</returns>
        Task<IList<User>> GetUsersByIds(string[] userIds);

        /// <summary>
        /// Gets a user by GUID
        /// </summary>
        /// <param name="userGuid">User GUID</param>
        /// <returns>A user</returns>
        Task<User> GetUserByGuid(Guid userGuid);

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User</returns>
        Task<User> GetUserByEmail(string email);

        /// <summary>
        /// Get user by system role
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>User</returns>
        Task<User> GetUserBySystemName(string systemName);

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User</returns>
        Task<User> GetUserByUsername(string username);

        /// <summary>
        /// Insert a user
        /// </summary>
        /// <param name="user">User</param>
        Task InsertUser(User user);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        Task UpdateUser(User user);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        Task UpdateUserLastActivityDate(User user);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        Task UpdateUserPassword(User user);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        Task UpdateActive(User user);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        Task UpdateUserLastLoginDate(User user);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        Task UpdateUserLastIpAddress(User user);

        #endregion

        #region Password history

        /// <summary>
        /// Gets user passwords
        /// </summary>
        /// <param name="userId">User identifier; pass null to load all records</param>
        /// <param name="passwordsToReturn">Number of returning passwords; pass null to load all records</param>
        /// <returns>List of user passwords</returns>
        Task<IList<UserHistoryPassword>> GetPasswords(string userId, int passwordsToReturn);

        /// <summary>
        /// Insert a user history password
        /// </summary>
        /// <param name="user">User</param>
        Task InsertUserPassword(User user);


        #endregion

    }
}