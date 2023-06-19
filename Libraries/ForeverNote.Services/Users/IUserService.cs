using ForeverNote.Core;
using ForeverNote.Core.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    /// <summary>
    /// User service interface
    /// </summary>
    public interface IUserService
    {
        #region Users

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="affiliateId">Affiliate identifier</param>
        /// <param name="vendorId">Vendor identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="ownerId">Owner identifier</param>
        /// <param name="salesEmployeeId">Sales employee identifier</param>
        /// <param name="userGroupIds">A list of user group identifiers to filter by (at least one match); pass null or empty list in order to load all users; </param>
        /// <param name="userTagIds"></param>
        /// <param name="email">Email; null to load all users</param>
        /// <param name="username">Username; null to load all users</param>
        /// <param name="firstName">First name; null to load all users</param>
        /// <param name="lastName">Last name; null to load all users</param>
        /// <param name="company">Company; null to load all users</param>
        /// <param name="phone">Phone; null to load all users</param>
        /// <param name="zipPostalCode">Phone; null to load all users</param>
        /// <param name="loadOnlyWithShoppingCart">Value indicating whether to load users only with shopping cart</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="orderBySelector"></param>
        /// <returns>Users</returns>
        Task<IPagedList<User>> GetAllUsers(DateTime? createdFromUtc = null,
            DateTime? createdToUtc = null, string affiliateId = "", string vendorId = "", string storeId = "", string ownerId = "",
            string salesEmployeeId = "", string[] userGroupIds = null, string[] userTagIds = null, string email = null, string username = null,
            string firstName = null, string lastName = null,
            string company = null, string phone = null, string zipPostalCode = null,
            bool loadOnlyWithShoppingCart = false,
            int pageIndex = 0, int pageSize = int.MaxValue, Expression<Func<User, object>> orderBySelector = null);

        
        /// <summary>
        /// Gets online users
        /// </summary>
        /// <param name="lastActivityFromUtc">User last activity date (from)</param>
        /// <param name="userGroupIds">A list of user group identifiers to filter by (at least one match); pass null or empty list in order to load all users; </param>
        /// <param name="storeId">Store ident</param>
        /// <param name="salesEmployeeId">Sales employee ident</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Users</returns>
        Task<IPagedList<User>> GetOnlineUsers(DateTime lastActivityFromUtc,
            string[] userGroupIds, string storeId = "", string salesEmployeeId = "", int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets count online users
        /// </summary>
        /// <param name="lastActivityFromUtc">User last activity date (from)</param>
        /// <param name="storeId">Store ident</param>
        /// <param name="salesEmployeeId">Sales employee ident</param>
        /// <returns>Int</returns>
        Task<int> GetCountOnlineShoppingCart(DateTime lastActivityFromUtc, string storeId = "", string salesEmployeeId = "");

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
        /// Get user by system group
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
        /// Delete a user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="hard">Hard delete from database</param>
        Task DeleteUser(User user, bool hard = false);

        /// <summary>
        /// Updates the user field
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        Task UpdateGenericAttribute<T>(User user,
            Expression<Func<User, T>> expression, T value);

        /// <summary>
        /// Updates the user field
        /// </summary>
        /// <param name="userId">User ident</param>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        Task UpdateGenericAttribute<T>(string userId,
            Expression<Func<User, T>> expression, T value);
      
        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        Task UpdateActive(User user);

        /////// <summary>
        /////// Update the user
        /////// </summary>
        /////// <param name="user"></param>
        ////Task UpdateContributions(User user);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        Task UpdateUserLastLoginDate(User user);
        
        /// <summary>
        /// Updates the user in admin panel
        /// </summary>
        /// <param name="user">User</param>
        Task UpdateUserInAdminPanel(User user);

        /// <summary>
        /// Reset data required for checkout
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="clearCouponCodes">A value indicating whether to clear coupon code</param>
        /// <param name="clearCheckoutAttributes">A value indicating whether to clear selected checkout attributes</param>
        /// <param name="clearLoyaltyPoints">A value indicating whether to clear "Use loyalty points" flag</param>
        /// <param name="clearShipping">A value indicating whether to clear selected shipping method</param>
        /// <param name="clearPayment">A value indicating whether to clear selected payment method</param>
        ////Task ResetCheckoutData(User user, string storeId,
        ////    bool clearCouponCodes = false, bool clearCheckoutAttributes = false,
        ////    bool clearLoyaltyPoints = true, bool clearShipping = true, bool clearPayment = true);

        /// <summary>
        /// Delete guest user records
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="onlyWithoutShoppingCart">A value indicating whether to delete users only without shopping cart</param>
        /// <returns>Number of deleted users</returns>
        ////Task<int> DeleteGuestUsers(DateTime? createdFromUtc, DateTime? createdToUtc, bool onlyWithoutShoppingCart);

        #endregion

        ////#region User Group in User

        ////Task InsertUserGroupInUser(UserGroup userGroup, string userId);

        ////Task DeleteUserGroupInUser(UserGroup userGroup, string userId);

        ////#endregion

        ////#region User address

        ////Task DeleteAddress(Address address, string userId);
        ////Task InsertAddress(Address address, string userId);
        ////Task UpdateAddress(Address address, string userId);
        ////Task UpdateBillingAddress(Address address, string userId);
        ////Task UpdateShippingAddress(Address address, string userId);

        ////#endregion

        ////#region Shopping cart 

        ////Task ClearShoppingCartItem(string userId, IList<ShoppingCartItem> cart);
        ////Task DeleteShoppingCartItem(string userId, ShoppingCartItem shoppingCartItem);
        ////Task InsertShoppingCartItem(string userId, ShoppingCartItem shoppingCartItem);
        ////Task UpdateShoppingCartItem(string userId, ShoppingCartItem shoppingCartItem);
        ////#endregion

    }
}