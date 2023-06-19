using ForeverNote.Core;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Queries.Users;
using ForeverNote.Services.Common;
using ForeverNote.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    /// <summary>
    /// User service
    /// </summary>
    public class UserService : IUserService
    {
        #region Fields

        private readonly IRepository<User> _userRepository;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IMediator _mediator;

        #endregion

        #region Ctor

        public UserService(
            IRepository<User> userRepository,
            IGenericAttributeService genericAttributeService,
            IMediator mediator)
        {
            _userRepository = userRepository;
            _genericAttributeService = genericAttributeService;
            _mediator = mediator;
        }

        #endregion

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
        /// <param name="userTagIds">user tags ids</param>
        /// <param name="email">Email; null to load all users</param>
        /// <param name="username">Username; null to load all users</param>
        /// <param name="firstName">First name; null to load all users</param>
        /// <param name="lastName">Last name; null to load all users</param>
        /// <param name="company">Company; null to load all users</param>
        /// <param name="phone">Phone; null to load all users</param>
        /// <param name="zipPostalCode">Phone; null to load all users</param>
        /// <param name="loadOnlyWithShoppingCart">Value indicating whether to load users only with shopping cart</param>
        /// <param name="sct">Value indicating what shopping cart type to filter; used when 'loadOnlyWithShoppingCart' param is 'true'</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="orderBySelector">order by selector</param>
        /// <returns>Users</returns>
        public virtual async Task<IPagedList<User>> GetAllUsers(DateTime? createdFromUtc = null,
            DateTime? createdToUtc = null, string affiliateId = "", string vendorId = "", string storeId = "", string ownerId = "",
            string salesEmployeeId = "", string[] userGroupIds = null, string[] userTagIds = null, string email = null, string username = null,
            string firstName = null, string lastName = null,
            string company = null, string phone = null, string zipPostalCode = null,
            bool loadOnlyWithShoppingCart = false,
            int pageIndex = 0, int pageSize = 2147483647, Expression<Func<User, object>> orderBySelector = null)
        {
            var queryModel = new GetUserQuery {
                CreatedFromUtc = createdFromUtc,
                CreatedToUtc = createdToUtc,
                AffiliateId = affiliateId,
                VendorId = vendorId,
                StoreId = storeId,
                OwnerId = ownerId,
                SalesEmployeeId = salesEmployeeId,
                UserGroupIds = userGroupIds,
                UserTagIds = userTagIds,
                Email = email,
                Username = username,
                FirstName = firstName,
                LastName = lastName,
                Company = company,
                Phone = phone,
                ZipPostalCode = zipPostalCode,
                LoadOnlyWithShoppingCart = loadOnlyWithShoppingCart,
                PageIndex = pageIndex,
                PageSize = pageSize,
                OrderBySelector = orderBySelector
            };
            var query = await _mediator.Send(queryModel);
            return await PagedList<User>.Create(query, pageIndex, pageSize);
        }       

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
        public virtual async Task<IPagedList<User>> GetOnlineUsers(DateTime lastActivityFromUtc,
            string[] userGroupIds, string storeId = "", string salesEmployeeId = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from p in _userRepository.Table
                        select p;

            query = query.Where(c => lastActivityFromUtc <= c.LastActivityDateUtc);
            query = query.Where(c => !c.Deleted);

            ////if (userGroupIds is { Length: > 0 })
            ////    query = query.Where(c => c.Groups.Select(cr => cr).Intersect(userGroupIds).Any());

            ////if (!string.IsNullOrEmpty(storeId))
            ////    query = query.Where(c => c.StoreId == storeId);

            ////if (!string.IsNullOrEmpty(salesEmployeeId))
            ////    query = query.Where(c => c.SeId == salesEmployeeId);

            query = query.OrderByDescending(c => c.LastActivityDateUtc);
            return await PagedList<User>.Create(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets count online users
        /// </summary>
        /// <param name="lastActivityFromUtc">User last activity date (from)</param>
        /// <param name="storeId">Store ident</param>
        /// <param name="salesEmployeeId">Sales employee ident</param>
        /// <returns>Int</returns>
        public virtual async Task<int> GetCountOnlineShoppingCart(DateTime lastActivityFromUtc, string storeId = "", string salesEmployeeId = "")
        {
            var query = from p in _userRepository.Table
                        select p;

            query = query.Where(c => c.Active);
            ////query = query.Where(c => lastActivityFromUtc <= c.LastUpdateCartDateUtc);
            ////query = query.Where(c => c.ShoppingCartItems.Any(y => y.ShoppingCartTypeId == ShoppingCartType.ShoppingCart));

            ////if (!string.IsNullOrEmpty(storeId))
            ////    query = query.Where(c => c.StoreId == storeId);

            ////if (!string.IsNullOrEmpty(salesEmployeeId))
            ////    query = query.Where(c => c.SeId == salesEmployeeId);

            return await Task.FromResult(query.Count());
        }

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>A user</returns>
        public virtual Task<User> GetUserById(string userId)
        {
            return string.IsNullOrWhiteSpace(userId) ? Task.FromResult<User>(null) : _userRepository.GetByIdAsync(userId);
        }

        /// <summary>
        /// Get users by identifiers
        /// </summary>
        /// <param name="userIds">User identifiers</param>
        /// <returns>Users</returns>
        public virtual async Task<IList<User>> GetUsersByIds(string[] userIds)
        {
            if (userIds == null || userIds.Length == 0)
                return new List<User>();

            var query = from c in _userRepository.Table
                        where userIds.Contains(c.Id)
                        select c;
            var users = query.ToList();
            //sort by passed identifiers
            var sortedUsers = userIds.Select(id => users.Find(x => x.Id == id)).Where(user => user != null).ToList();
            return await Task.FromResult(sortedUsers);
        }

        /// <summary>
        /// Gets a user by GUID
        /// </summary>
        /// <param name="userGuid">User GUID</param>
        /// <returns>A user</returns>
        public virtual async Task<User> GetUserByGuid(Guid userGuid)
        {
            return await Task.FromResult(_userRepository.Table.FirstOrDefault(x => x.UserGuid == userGuid));
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User</returns>
        public virtual async Task<User> GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return await Task.FromResult(_userRepository.Table.FirstOrDefault(x => x.Email == email.ToLowerInvariant()));
        }

        /// <summary>
        /// Get user by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>User</returns>
        public virtual async Task<User> GetUserBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            return await Task.FromResult(_userRepository.Table.FirstOrDefault(x => x.SystemName == systemName));
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User</returns>
        public virtual async Task<User> GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            return await Task.FromResult(_userRepository.Table.FirstOrDefault(x => x.Username == username.ToLowerInvariant()));
        }

        /// <summary>
        /// Insert a guest user
        /// </summary>
        /// <returns>User</returns>
        ////public virtual async Task<User> InsertGuestUser(Store store)
        ////{
        ////    var user = new User {
        ////        UserGuid = Guid.NewGuid(),
        ////        Active = true,
        ////        StoreId = store.Id,
        ////        CreatedOnUtc = DateTime.UtcNow,
        ////        LastActivityDateUtc = DateTime.UtcNow
        ////    };
        ////    //add to 'Guests' group
        ////    var guestGroup = await _mediator.Send(new GetGroupBySystemNameQuery { SystemName = SystemUserGroupNames.Guests });
        ////    if (guestGroup == null)
        ////        throw new ForeverNoteException("'Guests' group could not be loaded");
        ////    user.Groups.Add(guestGroup.Id);

        ////    await _userRepository.InsertAsync(user);

        ////    //event notification
        ////    await _mediator.EntityInserted(user);

        ////    return user;
        ////}

        /// <summary>
        /// Insert a user
        /// </summary>
        /// <param name="user">User</param>
        public virtual async Task InsertUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!string.IsNullOrEmpty(user.Email))
                user.Email = user.Email.ToLowerInvariant();

            if (!string.IsNullOrEmpty(user.Username))
                user.Username = user.Username.ToLowerInvariant();

            await _userRepository.InsertAsync(user);

            //event notification
            await _mediator.EntityInserted(user);
        }

        /// <summary>
        /// Updates the user field
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="expression">expression</param>
        /// <param name="value">value</param>
        public virtual async Task UpdateGenericAttribute<T>(User user,
            Expression<Func<User, T>> expression, T value)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            await UpdateGenericAttribute(user.Id, expression, value);

        }

        /// <summary>
        /// Updates the user field
        /// </summary>
        /// <param name="userId">User ident</param>
        /// <param name="expression">Expression</param>
        /// <param name="value">value</param>
        public virtual async Task UpdateGenericAttribute<T>(string userId,
            Expression<Func<User, T>> expression, T value)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            await _userRepository.UpdateField(userId, expression, value);

        }
        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        public virtual async Task UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (user.IsSystemAccount)
                throw new ForeverNoteException($"System user account ({(string.IsNullOrEmpty(user.SystemName) ? user.Email : user.SystemName)}) could not be updated");

            var update = UpdateBuilder<User>.Create()
                .Set(x => x.Email, string.IsNullOrEmpty(user.Email) ? "" : user.Email.ToLowerInvariant())
                .Set(x => x.PasswordFormatId, user.PasswordFormatId)
                .Set(x => x.PasswordSalt, user.PasswordSalt)
                .Set(x => x.Active, user.Active)
                ////.Set(x => x.StoreId, user.StoreId)
                .Set(x => x.Password, user.Password)
                .Set(x => x.PasswordChangeDateUtc, user.PasswordChangeDateUtc)
                .Set(x => x.Username, string.IsNullOrEmpty(user.Username) ? "" : user.Username.ToLowerInvariant())
                .Set(x => x.Deleted, user.Deleted);

            await _userRepository.UpdateOneAsync(x => x.Id == user.Id, update);

            //event notification
            await _mediator.EntityUpdated(user);
        }


        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="hard">Hard delete from database</param>
        public virtual async Task DeleteUser(User user, bool hard = false)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (user.IsSystemAccount)
                throw new ForeverNoteException($"System user account ({(string.IsNullOrEmpty(user.SystemName) ? user.Email : user.SystemName)}) could not be deleted");

            user.Deleted = true;
            user.Email = $"DELETED_@{DateTime.UtcNow.Ticks}.COM";
            user.Username = user.Email;

            //////delete address
            ////user.Addresses.Clear();
            ////user.BillingAddress = null;
            ////user.ShippingAddress = null;
            //delete user fields
            user.GenericAttributes.Clear();
            //////delete shopping cart
            ////user.ShoppingCartItems.Clear();
            //////delete user groups
            ////user.Groups.Clear();
            //clear user tags
            user.UserTags.Clear();
            //update user
            await _userRepository.UpdateAsync(user);

            if (hard)
                await _userRepository.DeleteAsync(user);

            //event notification
            await _mediator.EntityDeleted(user);

        }

        /// <summary>
        /// Updates the user - last activity date
        /// </summary>
        /// <param name="user">User</param>
        public virtual async Task UpdateUserLastLoginDate(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var update = UpdateBuilder<User>.Create()
                .Set(x => x.LastLoginDateUtc, user.LastLoginDateUtc)
                .Set(x => x.FailedLoginAttempts, user.FailedLoginAttempts)
                .Set(x => x.CannotLoginUntilDateUtc, user.CannotLoginUntilDateUtc);

            await _userRepository.UpdateOneAsync(x => x.Id == user.Id, update);

        }

        public virtual async Task UpdateUserInAdminPanel(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (user.IsSystemAccount)
                throw new ForeverNoteException($"System user account ({(string.IsNullOrEmpty(user.SystemName) ? user.Email : user.SystemName)}) could not be updated");

            var update = UpdateBuilder<User>.Create()
                .Set(x => x.Active, user.Active)
                ////.Set(x => x.AdminComment, user.AdminComment)
                ////.Set(x => x.AffiliateId, user.AffiliateId)
                .Set(x => x.Active, user.Active)
                .Set(x => x.Email, string.IsNullOrEmpty(user.Email) ? "" : user.Email.ToLowerInvariant())
                ////.Set(x => x.IsTaxExempt, user.IsTaxExempt)
                .Set(x => x.Password, user.Password)
                .Set(x => x.Username, string.IsNullOrEmpty(user.Username) ? "" : user.Username.ToLowerInvariant())
                ////.Set(x => x.Groups, user.Groups)
                ////.Set(x => x.Addresses, user.Addresses)
                ////.Set(x => x.FreeShipping, user.FreeShipping)
                ////.Set(x => x.VendorId, user.VendorId)
                ////.Set(x => x.SeId, user.SeId)
                ////.Set(x => x.OwnerId, user.OwnerId)
                ////.Set(x => x.StaffStoreId, user.StaffStoreId)
                ////.Set(x => x.Attributes, user.Attributes)
                ;

            await _userRepository.UpdateOneAsync(x => x.Id == user.Id, update);
            //event notification
            await _mediator.EntityUpdated(user);

        }


        public virtual async Task UpdateActive(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var update = UpdateBuilder<User>.Create()
                .Set(x => x.Active, user.Active)
                ////.Set(x => x.StoreId, user.StoreId)
                ;

            await _userRepository.UpdateOneAsync(x => x.Id == user.Id, update);

            //event notification
            await _mediator.EntityUpdated(user);
        }

        ////public virtual async Task UpdateContributions(User user)
        ////{
        ////    if (user == null)
        ////        throw new ArgumentNullException(nameof(user));

        ////    await UpdateGenericAttribute(user.Id, x => x.HasContributions, true);

        ////    //event notification
        ////    await _mediator.EntityUpdated(user);
        ////}

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
        ////public virtual async Task ResetCheckoutData(User user, string storeId,
        ////    bool clearCouponCodes = false, bool clearCheckoutAttributes = false,
        ////    bool clearLoyaltyPoints = true, bool clearShipping = true, bool clearPayment = true)
        ////{
        ////    if (user == null)
        ////        throw new ArgumentNullException();

        ////    //clear entered coupon codes
        ////    if (clearCouponCodes)
        ////    {
        ////        await _genericAttributeService.SaveField<string>(user, SystemGenericAttributeNames.DiscountCoupons, null);
        ////        await _genericAttributeService.SaveField<string>(user, SystemGenericAttributeNames.GiftVoucherCoupons, null);
        ////    }

        ////    //clear checkout attributes
        ////    if (clearCheckoutAttributes)
        ////    {
        ////        await _genericAttributeService.SaveField<string>(user, SystemGenericAttributeNames.CheckoutAttributes, null, storeId);
        ////    }

        ////    //clear loyalty points flag
        ////    if (clearLoyaltyPoints)
        ////    {
        ////        await _genericAttributeService.SaveField(user, SystemGenericAttributeNames.UseLoyaltyPointsDuringCheckout, false, storeId);
        ////    }

        ////    //clear selected shipping method
        ////    if (clearShipping)
        ////    {
        ////        await _genericAttributeService.SaveField<ShippingOption>(user, SystemGenericAttributeNames.SelectedShippingOption, null, storeId);
        ////        await _genericAttributeService.SaveField<ShippingOption>(user, SystemGenericAttributeNames.OfferedShippingOptions, null, storeId);
        ////        await _genericAttributeService.SaveField(user, SystemGenericAttributeNames.SelectedPickupPoint, "", storeId);
        ////        await _genericAttributeService.SaveField(user, SystemGenericAttributeNames.ShippingOptionAttributeDescription, "", storeId);
        ////        await _genericAttributeService.SaveField(user, SystemGenericAttributeNames.ShippingOptionAttribute, "", storeId);
        ////    }

        ////    //clear selected payment method
        ////    if (clearPayment)
        ////    {
        ////        await _genericAttributeService.SaveField<string>(user, SystemGenericAttributeNames.SelectedPaymentMethod, null, storeId);
        ////        await _genericAttributeService.SaveField<string>(user, SystemGenericAttributeNames.PaymentTransaction, null, storeId);
        ////        await _genericAttributeService.SaveField(user, SystemGenericAttributeNames.PaymentOptionAttribute, "", storeId);
        ////    }
        ////}

        /// <summary>
        /// Delete guest user records
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="onlyWithoutShoppingCart">A value indicating whether to delete users only without shopping cart</param>
        /// <returns>Number of deleted users</returns>
        ////public virtual async Task<int> DeleteGuestUsers(DateTime? createdFromUtc, DateTime? createdToUtc, bool onlyWithoutShoppingCart)
        ////{
        ////    var guestGroup = await _mediator.Send(new GetGroupBySystemNameQuery { SystemName = SystemUserGroupNames.Guests });
        ////    if (guestGroup == null)
        ////        throw new ForeverNoteException("Guests group could not be loaded");

        ////    var query = from p in _userRepository.Table
        ////                select p;

        ////    query = query.Where(x => x.Groups.Contains(guestGroup.Id));

        ////    if (createdFromUtc.HasValue)
        ////        query = query.Where(x => x.LastActivityDateUtc > createdFromUtc.Value);
        ////    if (createdToUtc.HasValue)
        ////        query = query.Where(x => x.LastActivityDateUtc < createdToUtc.Value);
        ////    if (onlyWithoutShoppingCart)
        ////        query = query.Where(x => !x.ShoppingCartItems.Any());

        ////    query = query.Where(x => !x.HasContributions);

        ////    query = query.Where(x => !x.IsSystemAccount);

        ////    var users = await _userRepository.DeleteAsync(query);

        ////    return users.Count();

        ////}

        #endregion

        ////#region User group in user

        ////public virtual async Task DeleteUserGroupInUser(UserGroup userGroup, string userId)
        ////{
        ////    if (userGroup == null)
        ////        throw new ArgumentNullException(nameof(userGroup));

        ////    if (string.IsNullOrEmpty(userId))
        ////        throw new ArgumentNullException(nameof(userId));

        ////    await _userRepository.Pull(userId, x => x.Groups, userGroup.Id);
        ////}

        ////public virtual async Task InsertUserGroupInUser(UserGroup userGroup, string userId)
        ////{
        ////    if (userGroup == null)
        ////        throw new ArgumentNullException(nameof(userGroup));

        ////    if (string.IsNullOrEmpty(userId))
        ////        throw new ArgumentNullException(nameof(userId));

        ////    await _userRepository.AddToSet(userId, x => x.Groups, userGroup.Id);

        ////}

        ////#endregion

        ////#region User Address

        ////public virtual async Task DeleteAddress(Address address, string userId)
        ////{
        ////    if (address == null)
        ////        throw new ArgumentNullException(nameof(address));

        ////    if (string.IsNullOrEmpty(userId))
        ////        throw new ArgumentNullException(nameof(userId));

        ////    await _userRepository.PullFilter(userId, x => x.Addresses, x => x.Id, address.Id);

        ////    //event notification
        ////    await _mediator.EntityDeleted(address);

        ////}

        ////public virtual async Task InsertAddress(Address address, string userId)
        ////{
        ////    if (address == null)
        ////        throw new ArgumentNullException(nameof(address));

        ////    if (string.IsNullOrEmpty(userId))
        ////        throw new ArgumentNullException(nameof(userId));

        ////    if (address.StateProvinceId == "0")
        ////        address.StateProvinceId = "";

        ////    await _userRepository.AddToSet(userId, x => x.Addresses, address);

        ////    //event notification
        ////    await _mediator.EntityInserted(address);
        ////}

        ////public virtual async Task UpdateAddress(Address address, string userId)
        ////{
        ////    if (address == null)
        ////        throw new ArgumentNullException(nameof(address));

        ////    if (string.IsNullOrEmpty(userId))
        ////        throw new ArgumentNullException(nameof(userId));

        ////    await _userRepository.UpdateToSet(userId, x => x.Addresses, z => z.Id, address.Id, address);

        ////    //event notification
        ////    await _mediator.EntityUpdated(address);
        ////}


        ////public virtual async Task UpdateBillingAddress(Address address, string userId)
        ////{
        ////    if (address == null)
        ////        throw new ArgumentNullException(nameof(address));

        ////    if (string.IsNullOrEmpty(userId))
        ////        throw new ArgumentNullException(nameof(userId));

        ////    await _userRepository.UpdateField(userId, x => x.BillingAddress, address);

        ////}
        ////public virtual async Task UpdateShippingAddress(Address address, string userId)
        ////{
        ////    if (address == null)
        ////        throw new ArgumentNullException(nameof(address));

        ////    if (string.IsNullOrEmpty(userId))
        ////        throw new ArgumentNullException(nameof(userId));

        ////    await _userRepository.UpdateField(userId, x => x.ShippingAddress, address);
        ////}

        ////#endregion

        ////#region User Shopping Cart Item

        ////public virtual async Task DeleteShoppingCartItem(string userId, ShoppingCartItem shoppingCartItem)
        ////{
        ////    if (shoppingCartItem == null)
        ////        throw new ArgumentNullException(nameof(shoppingCartItem));

        ////    await _userRepository.PullFilter(userId, x => x.ShoppingCartItems, x => x.Id, shoppingCartItem.Id);

        ////    if (shoppingCartItem.ShoppingCartTypeId == ShoppingCartType.ShoppingCart)
        ////        await UpdateGenericAttribute(userId, x => x.LastUpdateCartDateUtc, DateTime.UtcNow);
        ////    else
        ////        await UpdateGenericAttribute(userId, x => x.LastUpdateWishListDateUtc, DateTime.UtcNow);

        ////}

        ////public virtual async Task ClearShoppingCartItem(string userId, IList<ShoppingCartItem> cart)
        ////{
        ////    foreach (var item in cart)
        ////    {
        ////        await _userRepository.PullFilter(userId, x => x.ShoppingCartItems, x => x.Id, item.Id);
        ////    }

        ////    if (cart.Any(c => c.ShoppingCartTypeId is ShoppingCartType.ShoppingCart or ShoppingCartType.Auctions))
        ////        await UpdateGenericAttribute(userId, x => x.LastUpdateCartDateUtc, DateTime.UtcNow);
        ////    if (cart.Any(c => c.ShoppingCartTypeId == ShoppingCartType.Wishlist))
        ////        await UpdateGenericAttribute(userId, x => x.LastUpdateWishListDateUtc, DateTime.UtcNow);
        ////}

        ////public virtual async Task InsertShoppingCartItem(string userId, ShoppingCartItem shoppingCartItem)
        ////{
        ////    if (shoppingCartItem == null)
        ////        throw new ArgumentNullException(nameof(shoppingCartItem));

        ////    await _userRepository.AddToSet(userId, x => x.ShoppingCartItems, shoppingCartItem);

        ////    if (shoppingCartItem.ShoppingCartTypeId == ShoppingCartType.ShoppingCart)
        ////        await UpdateGenericAttribute(userId, x => x.LastUpdateCartDateUtc, DateTime.UtcNow);
        ////    else
        ////        await UpdateGenericAttribute(userId, x => x.LastUpdateWishListDateUtc, DateTime.UtcNow);
        ////}

        ////public virtual async Task UpdateShoppingCartItem(string userId, ShoppingCartItem shoppingCartItem)
        ////{
        ////    if (shoppingCartItem == null)
        ////        throw new ArgumentNullException(nameof(shoppingCartItem));

        ////    await _userRepository.UpdateToSet(userId, x => x.ShoppingCartItems, z => z.Id, shoppingCartItem.Id, shoppingCartItem);

        ////    if (shoppingCartItem.ShoppingCartTypeId == ShoppingCartType.ShoppingCart)
        ////        await UpdateGenericAttribute(userId, x => x.LastUpdateCartDateUtc, DateTime.UtcNow);
        ////    else
        ////        await UpdateGenericAttribute(userId, x => x.LastUpdateWishListDateUtc, DateTime.UtcNow);

        ////}
        ////#endregion
    }
}