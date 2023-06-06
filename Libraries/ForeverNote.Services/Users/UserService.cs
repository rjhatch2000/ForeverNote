using ForeverNote.Core;
using ForeverNote.Core.Caching;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Services.Common;
using ForeverNote.Services.Events;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    /// <summary>
    /// User service
    /// </summary>
    public partial class UserService : IUserService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        private const string CUSTOMERROLES_BY_SYSTEMNAME_KEY = "ForeverNote.userrole.systemname-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CUSTOMERROLES_PATTERN_KEY = "ForeverNote.userrole.";
        private const string CUSTOMERROLESPRODUCTS_PATTERN_KEY = "ForeverNote.note.cr";

       
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user role Id?
        /// </remarks>
        private const string CUSTOMERROLESPRODUCTS_ROLE_KEY = "ForeverNote.userrolenotes.role-{0}";

        #endregion

        #region Fields

        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserHistoryPassword> _userHistoryPasswordNoteRepository;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICacheManager _cacheManager;
        private readonly IMediator _mediator;

        #endregion

        #region Ctor

        public UserService(ICacheManager cacheManager,
            IRepository<User> userRepository,
            IRepository<UserHistoryPassword> userHistoryPasswordNoteRepository,
            IGenericAttributeService genericAttributeService,
            IMediator mediator)
        {
            _cacheManager = cacheManager;
            _userRepository = userRepository;
            _userHistoryPasswordNoteRepository = userHistoryPasswordNoteRepository;
            _genericAttributeService = genericAttributeService;
            _mediator = mediator;
        }

        #endregion

        #region Methods

        #region Users

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="affiliateId">Affiliate identifier</param>
        /// <param name="vendorId">Vendor identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="userRoleIds">A list of user role identifiers to filter by (at least one match); pass null or empty list in order to load all users; </param>
        /// <param name="email">Email; null to load all users</param>
        /// <param name="username">Username; null to load all users</param>
        /// <param name="firstName">First name; null to load all users</param>
        /// <param name="lastName">Last name; null to load all users</param>
        /// <param name="dayOfBirth">Day of birth; 0 to load all users</param>
        /// <param name="monthOfBirth">Month of birth; 0 to load all users</param>
        /// <param name="company">Company; null to load all users</param>
        /// <param name="phone">Phone; null to load all users</param>
        /// <param name="zipPostalCode">Phone; null to load all users</param>
        /// <param name="loadOnlyWithShoppingCart">Value indicating whether to load users only with shopping cart</param>
        /// <param name="sct">Value indicating what shopping cart type to filter; userd when 'loadOnlyWithShoppingCart' param is 'true'</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Users</returns>
        public virtual async Task<IPagedList<User>> GetAllUsers(DateTime? createdFromUtc = null,
            DateTime? createdToUtc = null,
            string[] userTagIds = null, string email = null, string username = null,
            string firstName = null, string lastName = null,
            string company = null, string phone = null, string zipPostalCode = null,
            int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _userRepository.Table;

            if (createdFromUtc.HasValue)
                query = query.Where(c => createdFromUtc.Value <= c.CreatedOnUtc);
            if (createdToUtc.HasValue)
                query = query.Where(c => createdToUtc.Value >= c.CreatedOnUtc);

            query = query.Where(c => !c.Deleted);
            if (userTagIds != null && userTagIds.Length > 0)
            {
                foreach (var item in userTagIds)
                {
                    query = query.Where(c => c.UserTags.Contains(item));
                }
            }
            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(c => c.Email != null && c.Email.Contains(email.ToLower()));
            if (!string.IsNullOrWhiteSpace(username))
                query = query.Where(c => c.Username != null && c.Username.ToLower().Contains(username.ToLower()));

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query.Where(x => x.GenericAttributes.Any(y => y.Key == SystemUserAttributeNames.FirstName && y.Value != null && y.Value.ToLower().Contains(firstName.ToLower())));
            }

            if (!String.IsNullOrWhiteSpace(lastName))
            {
                query = query.Where(x => x.GenericAttributes.Any(y => y.Key == SystemUserAttributeNames.LastName && y.Value != null && y.Value.ToLower().Contains(lastName.ToLower())));
            }

            //search by company
            if (!String.IsNullOrWhiteSpace(company))
            {
                query = query.Where(x => x.GenericAttributes.Any(y => y.Key == SystemUserAttributeNames.Company && y.Value != null && y.Value.ToLower().Contains(company.ToLower())));
            }
            //search by phone
            if (!String.IsNullOrWhiteSpace(phone))
            {
                query = query.Where(x => x.GenericAttributes.Any(y => y.Key == SystemUserAttributeNames.Phone && y.Value != null && y.Value.ToLower().Contains(phone.ToLower())));
            }
            //search by zip
            if (!String.IsNullOrWhiteSpace(zipPostalCode))
            {
                query = query.Where(x => x.GenericAttributes.Any(y => y.Key == SystemUserAttributeNames.ZipPostalCode && y.Value != null && y.Value.ToLower().Contains(zipPostalCode.ToLower())));
            }

            query = query.OrderByDescending(c => c.CreatedOnUtc);
            return await PagedList<User>.Create(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets all users by user format (including deleted ones)
        /// </summary>
        /// <param name="passwordFormat">Password format</param>
        /// <returns>Users</returns>
        public virtual async Task<IList<User>> GetAllUsersByPasswordFormat(PasswordFormat passwordFormat)
        {
            var passwordFormatId = (int)passwordFormat;

            var query = _userRepository.Table;
            query = query.Where(c => c.PasswordFormatId == passwordFormatId);
            query = query.OrderByDescending(c => c.CreatedOnUtc);
            return await query.ToListAsync();
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user">User</param>
        public virtual async Task DeleteUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (user.IsSystemAccount)
                throw new ForeverNoteException(string.Format("System user account ({0}) could not be deleted", user.SystemName));

            user.Deleted = true;
            user.Email = $"DELETED@{DateTime.UtcNow.Ticks}.COM";
            user.Username = user.Email;

            //delete generic attr
            user.GenericAttributes.Clear();
            //clear user tags
            user.UserTags.Clear();
            //update user
            await _userRepository.UpdateAsync(user);
        }

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>A user</returns>
        public virtual Task<User> GetUserById(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return Task.FromResult<User>(null);

            return _userRepository.GetByIdAsync(userId);
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
            var users = await query.ToListAsync();
            //sort by passed identifiers
            var sortedUsers = new List<User>();
            foreach (string id in userIds)
            {
                var user = users.Find(x => x.Id == id);
                if (user != null)
                    sortedUsers.Add(user);
            }
            return sortedUsers;
        }

        /// <summary>
        /// Gets a user by GUID
        /// </summary>
        /// <param name="userGuid">User GUID</param>
        /// <returns>A user</returns>
        public virtual Task<User> GetUserByGuid(Guid userGuid)
        {
            ////if (userGuid == null)
            ////    return Task.FromResult<User>(null);

            var filter = Builders<User>.Filter.Eq(x => x.UserGuid, userGuid);
            return _userRepository.Collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User</returns>
        public virtual Task<User> GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Task.FromResult<User>(null);

            var filter = Builders<User>.Filter.Eq(x => x.Email, email.ToLower());
            return _userRepository.Collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get user by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>User</returns>
        public virtual Task<User> GetUserBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return Task.FromResult<User>(null);

            var filter = Builders<User>.Filter.Eq(x => x.SystemName, systemName);
            return _userRepository.Collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User</returns>
        public virtual Task<User> GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return Task.FromResult<User>(null);

            var filter = Builders<User>.Filter.Eq(x => x.Username, username.ToLower());
            return _userRepository.Collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Insert a user
        /// </summary>
        /// <param name="user">User</param>
        public virtual async Task InsertUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (!string.IsNullOrEmpty(user.Email))
                user.Email = user.Email.ToLower();

            if (!string.IsNullOrEmpty(user.Username))
                user.Username = user.Username.ToLower();

            await _userRepository.InsertAsync(user);

            //event notification
            await _mediator.EntityInserted(user);
        }

        /// <summary>
        /// Insert a user history password
        /// </summary>
        /// <param name="user">User</param>
        public virtual async Task InsertUserPassword(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var chp = new UserHistoryPassword();
            chp.Password = user.Password;
            chp.PasswordFormatId = user.PasswordFormatId;
            chp.PasswordSalt = user.PasswordSalt;
            chp.UserId = user.Id;
            chp.CreatedOnUtc = DateTime.UtcNow;

            await _userHistoryPasswordNoteRepository.InsertAsync(chp);

            //event notification
            await _mediator.EntityInserted(chp);
        }

        /// <summary>
        /// Gets user passwords
        /// </summary>
        /// <param name="userId">User identifier; pass null to load all records</param>
        /// <param name="passwordsToReturn">Number of returning passwords; pass null to load all records</param>
        /// <returns>List of user passwords</returns>
        public virtual async Task<IList<UserHistoryPassword>> GetPasswords(string userId, int passwordsToReturn)
        {
            var filter = Builders<UserHistoryPassword>.Filter.Eq(x => x.UserId, userId);
            return await _userHistoryPasswordNoteRepository.Collection.Find(filter)
                    .SortByDescending(password => password.CreatedOnUtc)
                    .Limit(passwordsToReturn)
                    .ToListAsync();
        }

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        public virtual async Task UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var builder = Builders<User>.Filter;
            var filter = builder.Eq(x => x.Id, user.Id);
            var update = Builders<User>.Update
                .Set(x => x.Email, string.IsNullOrEmpty(user.Email) ? "" : user.Email.ToLower())
                .Set(x => x.PasswordFormatId, user.PasswordFormatId)
                .Set(x => x.PasswordSalt, user.PasswordSalt)
                .Set(x => x.Active, user.Active)
                .Set(x => x.Password, user.Password)
                .Set(x => x.PasswordChangeDateUtc, user.PasswordChangeDateUtc)
                .Set(x => x.Username, string.IsNullOrEmpty(user.Username) ? "" : user.Username.ToLower())
                .Set(x => x.Deleted, user.Deleted);

            await _userRepository.Collection.UpdateOneAsync(filter, update);

            //event notification
            await _mediator.EntityUpdated(user);
        }
        /// <summary>
        /// Updates the user - last activity date
        /// </summary>
        /// <param name="user">User</param>
        public virtual async Task UpdateUserLastActivityDate(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var builder = Builders<User>.Filter;
            var filter = builder.Eq(x => x.Id, user.Id);
            var update = Builders<User>.Update
                .Set(x => x.LastActivityDateUtc, user.LastActivityDateUtc);
            await _userRepository.Collection.UpdateOneAsync(filter, update);

        }
        /// <summary>
        /// Updates the user - last activity date
        /// </summary>
        /// <param name="user">User</param>
        public virtual async Task UpdateUserLastLoginDate(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var builder = Builders<User>.Filter;
            var filter = builder.Eq(x => x.Id, user.Id);
            var update = Builders<User>.Update
                .Set(x => x.LastLoginDateUtc, user.LastLoginDateUtc)
                .Set(x => x.FailedLoginAttempts, user.FailedLoginAttempts)
                .Set(x => x.CannotLoginUntilDateUtc, user.CannotLoginUntilDateUtc);

            await _userRepository.Collection.UpdateOneAsync(filter, update);

        }
        
        /// <summary>
        /// Updates the user - last activity date
        /// </summary>
        /// <param name="user">User</param>
        public virtual async Task UpdateUserLastIpAddress(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var builder = Builders<User>.Filter;
            var filter = builder.Eq(x => x.Id, user.Id);
            var update = Builders<User>.Update
                .Set(x => x.LastIpAddress, user.LastIpAddress);

            await _userRepository.Collection.UpdateOneAsync(filter, update);
        }
        /// <summary>
        /// Updates the user - password
        /// </summary>
        /// <param name="user">User</param>
        public virtual async Task UpdateUserPassword(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var builder = Builders<User>.Filter;
            var filter = builder.Eq(x => x.Id, user.Id);
            var update = Builders<User>.Update
                .Set(x => x.Password, user.Password);

            await _userRepository.Collection.UpdateOneAsync(filter, update);
        }
        public virtual async Task UpdateUserinAdminPanel(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var builder = Builders<User>.Filter;
            var filter = builder.Eq(x => x.Id, user.Id);
            var update = Builders<User>.Update
                .Set(x => x.Active, user.Active)
                .Set(x => x.IsSystemAccount, user.IsSystemAccount)
                .Set(x => x.Active, user.Active)
                .Set(x => x.Email, string.IsNullOrEmpty(user.Email) ? "" : user.Email.ToLower())
                .Set(x => x.Password, user.Password)
                .Set(x => x.SystemName, user.SystemName)
                .Set(x => x.Username, string.IsNullOrEmpty(user.Username) ? "" : user.Username.ToLower())
                ;

            await _userRepository.Collection.UpdateOneAsync(filter, update);
            //event notification
            await _mediator.EntityUpdated(user);

        }

        public virtual async Task UpdateActive(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(x => x.Id, user.Id);
            var update = Builders<User>.Update
                .Set(x => x.Active, user.Active);

            await _userRepository.Collection.UpdateOneAsync(filter, update);
        }

        #endregion

        #endregion
    }
}