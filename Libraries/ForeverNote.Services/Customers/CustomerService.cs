using ForeverNote.Core;
using ForeverNote.Core.Caching;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Services.Common;
using ForeverNote.Services.Events;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Customers
{
    /// <summary>
    /// Customer service
    /// </summary>
    public partial class CustomerService : ICustomerService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        private const string CUSTOMERROLES_BY_SYSTEMNAME_KEY = "ForeverNote.customerrole.systemname-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CUSTOMERROLES_PATTERN_KEY = "ForeverNote.customerrole.";
        private const string CUSTOMERROLESPRODUCTS_PATTERN_KEY = "ForeverNote.product.cr";

       
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer role Id?
        /// </remarks>
        private const string CUSTOMERROLESPRODUCTS_ROLE_KEY = "ForeverNote.customerroleproducts.role-{0}";

        #endregion

        #region Fields

        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerRole> _customerRoleRepository;
        private readonly IRepository<CustomerRoleProduct> _customerRoleProductRepository;
        private readonly IRepository<CustomerHistoryPassword> _customerHistoryPasswordProductRepository;
        private readonly IRepository<CustomerNote> _customerNoteRepository;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICacheManager _cacheManager;
        private readonly IMediator _mediator;

        #endregion

        #region Ctor

        public CustomerService(ICacheManager cacheManager,
            IRepository<Customer> customerRepository,
            IRepository<CustomerRole> customerRoleRepository,
            IRepository<CustomerHistoryPassword> customerHistoryPasswordProductRepository,
            IRepository<CustomerRoleProduct> customerRoleProductRepository,
            IRepository<CustomerNote> customerNoteRepository,
            IGenericAttributeService genericAttributeService,
            IMediator mediator)
        {
            _cacheManager = cacheManager;
            _customerRepository = customerRepository;
            _customerRoleRepository = customerRoleRepository;
            _customerHistoryPasswordProductRepository = customerHistoryPasswordProductRepository;
            _customerRoleProductRepository = customerRoleProductRepository;
            _customerNoteRepository = customerNoteRepository;
            _genericAttributeService = genericAttributeService;
            _mediator = mediator;
        }

        #endregion

        #region Methods

        #region Customers

        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="affiliateId">Affiliate identifier</param>
        /// <param name="vendorId">Vendor identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="customerRoleIds">A list of customer role identifiers to filter by (at least one match); pass null or empty list in order to load all customers; </param>
        /// <param name="email">Email; null to load all customers</param>
        /// <param name="username">Username; null to load all customers</param>
        /// <param name="firstName">First name; null to load all customers</param>
        /// <param name="lastName">Last name; null to load all customers</param>
        /// <param name="dayOfBirth">Day of birth; 0 to load all customers</param>
        /// <param name="monthOfBirth">Month of birth; 0 to load all customers</param>
        /// <param name="company">Company; null to load all customers</param>
        /// <param name="phone">Phone; null to load all customers</param>
        /// <param name="zipPostalCode">Phone; null to load all customers</param>
        /// <param name="loadOnlyWithShoppingCart">Value indicating whether to load customers only with shopping cart</param>
        /// <param name="sct">Value indicating what shopping cart type to filter; userd when 'loadOnlyWithShoppingCart' param is 'true'</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Customers</returns>
        public virtual async Task<IPagedList<Customer>> GetAllCustomers(DateTime? createdFromUtc = null,
            DateTime? createdToUtc = null,
            string[] customerRoleIds = null, string[] customerTagIds = null, string email = null, string username = null,
            string firstName = null, string lastName = null,
            string company = null, string phone = null, string zipPostalCode = null,
            int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _customerRepository.Table;

            if (createdFromUtc.HasValue)
                query = query.Where(c => createdFromUtc.Value <= c.CreatedOnUtc);
            if (createdToUtc.HasValue)
                query = query.Where(c => createdToUtc.Value >= c.CreatedOnUtc);

            query = query.Where(c => !c.Deleted);
            if (customerRoleIds != null && customerRoleIds.Length > 0)
                query = query.Where(c => c.CustomerRoles.Any(x => customerRoleIds.Contains(x.Id)));
            if (customerTagIds != null && customerTagIds.Length > 0)
            {
                foreach (var item in customerTagIds)
                {
                    query = query.Where(c => c.CustomerTags.Contains(item));
                }
            }
            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(c => c.Email != null && c.Email.Contains(email.ToLower()));
            if (!string.IsNullOrWhiteSpace(username))
                query = query.Where(c => c.Username != null && c.Username.ToLower().Contains(username.ToLower()));

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query.Where(x => x.GenericAttributes.Any(y => y.Key == SystemCustomerAttributeNames.FirstName && y.Value != null && y.Value.ToLower().Contains(firstName.ToLower())));
            }

            if (!String.IsNullOrWhiteSpace(lastName))
            {
                query = query.Where(x => x.GenericAttributes.Any(y => y.Key == SystemCustomerAttributeNames.LastName && y.Value != null && y.Value.ToLower().Contains(lastName.ToLower())));
            }

            //search by company
            if (!String.IsNullOrWhiteSpace(company))
            {
                query = query.Where(x => x.GenericAttributes.Any(y => y.Key == SystemCustomerAttributeNames.Company && y.Value != null && y.Value.ToLower().Contains(company.ToLower())));
            }
            //search by phone
            if (!String.IsNullOrWhiteSpace(phone))
            {
                query = query.Where(x => x.GenericAttributes.Any(y => y.Key == SystemCustomerAttributeNames.Phone && y.Value != null && y.Value.ToLower().Contains(phone.ToLower())));
            }
            //search by zip
            if (!String.IsNullOrWhiteSpace(zipPostalCode))
            {
                query = query.Where(x => x.GenericAttributes.Any(y => y.Key == SystemCustomerAttributeNames.ZipPostalCode && y.Value != null && y.Value.ToLower().Contains(zipPostalCode.ToLower())));
            }

            query = query.OrderByDescending(c => c.CreatedOnUtc);
            return await PagedList<Customer>.Create(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets all customers by customer format (including deleted ones)
        /// </summary>
        /// <param name="passwordFormat">Password format</param>
        /// <returns>Customers</returns>
        public virtual async Task<IList<Customer>> GetAllCustomersByPasswordFormat(PasswordFormat passwordFormat)
        {
            var passwordFormatId = (int)passwordFormat;

            var query = _customerRepository.Table;
            query = query.Where(c => c.PasswordFormatId == passwordFormatId);
            query = query.OrderByDescending(c => c.CreatedOnUtc);
            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets online customers
        /// </summary>
        /// <param name="lastActivityFromUtc">Customer last activity date (from)</param>
        /// <param name="customerRoleIds">A list of customer role identifiers to filter by (at least one match); pass null or empty list in order to load all customers; </param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="storeId">Store ident</param>
        /// <returns>Customers</returns>
        public virtual async Task<IPagedList<Customer>> GetOnlineCustomers(DateTime lastActivityFromUtc,
            string[] customerRoleIds, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _customerRepository.Table;
            query = query.Where(c => lastActivityFromUtc <= c.LastActivityDateUtc);
            query = query.Where(c => !c.Deleted);

            if (customerRoleIds != null && customerRoleIds.Length > 0)
                query = query.Where(c => c.CustomerRoles.Select(cr => cr.Id).Intersect(customerRoleIds).Any());

            query = query.OrderByDescending(c => c.LastActivityDateUtc);
            return await PagedList<Customer>.Create(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Delete a customer
        /// </summary>
        /// <param name="customer">Customer</param>
        public virtual async Task DeleteCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (customer.IsSystemAccount)
                throw new ForeverNoteException(string.Format("System customer account ({0}) could not be deleted", customer.SystemName));

            customer.Deleted = true;
            customer.Email = $"DELETED@{DateTime.UtcNow.Ticks}.COM";
            customer.Username = customer.Email;

            //delete generic attr
            customer.GenericAttributes.Clear();
            //delete customer roles
            customer.CustomerRoles.Clear();
            //clear customer tags
            customer.CustomerTags.Clear();
            //update customer
            await _customerRepository.UpdateAsync(customer);
        }

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>A customer</returns>
        public virtual Task<Customer> GetCustomerById(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return Task.FromResult<Customer>(null);

            return _customerRepository.GetByIdAsync(customerId);
        }

        /// <summary>
        /// Get customers by identifiers
        /// </summary>
        /// <param name="customerIds">Customer identifiers</param>
        /// <returns>Customers</returns>
        public virtual async Task<IList<Customer>> GetCustomersByIds(string[] customerIds)
        {
            if (customerIds == null || customerIds.Length == 0)
                return new List<Customer>();

            var query = from c in _customerRepository.Table
                        where customerIds.Contains(c.Id)
                        select c;
            var customers = await query.ToListAsync();
            //sort by passed identifiers
            var sortedCustomers = new List<Customer>();
            foreach (string id in customerIds)
            {
                var customer = customers.Find(x => x.Id == id);
                if (customer != null)
                    sortedCustomers.Add(customer);
            }
            return sortedCustomers;
        }

        /// <summary>
        /// Gets a customer by GUID
        /// </summary>
        /// <param name="customerGuid">Customer GUID</param>
        /// <returns>A customer</returns>
        public virtual Task<Customer> GetCustomerByGuid(Guid customerGuid)
        {
            ////if (customerGuid == null)
            ////    return Task.FromResult<Customer>(null);

            var filter = Builders<Customer>.Filter.Eq(x => x.CustomerGuid, customerGuid);
            return _customerRepository.Collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get customer by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Customer</returns>
        public virtual Task<Customer> GetCustomerByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Task.FromResult<Customer>(null);

            var filter = Builders<Customer>.Filter.Eq(x => x.Email, email.ToLower());
            return _customerRepository.Collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get customer by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Customer</returns>
        public virtual Task<Customer> GetCustomerBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return Task.FromResult<Customer>(null);

            var filter = Builders<Customer>.Filter.Eq(x => x.SystemName, systemName);
            return _customerRepository.Collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get customer by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Customer</returns>
        public virtual Task<Customer> GetCustomerByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return Task.FromResult<Customer>(null);

            var filter = Builders<Customer>.Filter.Eq(x => x.Username, username.ToLower());
            return _customerRepository.Collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Insert a guest customer
        /// </summary>
        /// <returns>Customer</returns>
        public virtual async Task<Customer> InsertGuestCustomer(string urlreferrer = "")
        {
            var customer = new Customer {
                CustomerGuid = Guid.NewGuid(),
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
                UrlReferrer = urlreferrer
            };

            //add to 'Guests' role
            var guestRole = await GetCustomerRoleBySystemName(SystemCustomerRoleNames.Guests);
            if (guestRole == null)
                throw new ForeverNoteException("'Guests' role could not be loaded");
            customer.CustomerRoles.Add(guestRole);

            await _customerRepository.InsertAsync(customer);

            return customer;
        }

        /// <summary>
        /// Insert a customer
        /// </summary>
        /// <param name="customer">Customer</param>
        public virtual async Task InsertCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (!string.IsNullOrEmpty(customer.Email))
                customer.Email = customer.Email.ToLower();

            if (!string.IsNullOrEmpty(customer.Username))
                customer.Username = customer.Username.ToLower();

            await _customerRepository.InsertAsync(customer);

            //event notification
            await _mediator.EntityInserted(customer);
        }

        /// <summary>
        /// Insert a customer history password
        /// </summary>
        /// <param name="customer">Customer</param>
        public virtual async Task InsertCustomerPassword(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var chp = new CustomerHistoryPassword();
            chp.Password = customer.Password;
            chp.PasswordFormatId = customer.PasswordFormatId;
            chp.PasswordSalt = customer.PasswordSalt;
            chp.CustomerId = customer.Id;
            chp.CreatedOnUtc = DateTime.UtcNow;

            await _customerHistoryPasswordProductRepository.InsertAsync(chp);

            //event notification
            await _mediator.EntityInserted(chp);
        }

        /// <summary>
        /// Gets customer passwords
        /// </summary>
        /// <param name="customerId">Customer identifier; pass null to load all records</param>
        /// <param name="passwordsToReturn">Number of returning passwords; pass null to load all records</param>
        /// <returns>List of customer passwords</returns>
        public virtual async Task<IList<CustomerHistoryPassword>> GetPasswords(string customerId, int passwordsToReturn)
        {
            var filter = Builders<CustomerHistoryPassword>.Filter.Eq(x => x.CustomerId, customerId);
            return await _customerHistoryPasswordProductRepository.Collection.Find(filter)
                    .SortByDescending(password => password.CreatedOnUtc)
                    .Limit(passwordsToReturn)
                    .ToListAsync();
        }

        /// <summary>
        /// Updates the customer
        /// </summary>
        /// <param name="customer">Customer</param>
        public virtual async Task UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var builder = Builders<Customer>.Filter;
            var filter = builder.Eq(x => x.Id, customer.Id);
            var update = Builders<Customer>.Update
                .Set(x => x.Email, string.IsNullOrEmpty(customer.Email) ? "" : customer.Email.ToLower())
                .Set(x => x.PasswordFormatId, customer.PasswordFormatId)
                .Set(x => x.PasswordSalt, customer.PasswordSalt)
                .Set(x => x.Active, customer.Active)
                .Set(x => x.Password, customer.Password)
                .Set(x => x.PasswordChangeDateUtc, customer.PasswordChangeDateUtc)
                .Set(x => x.Username, string.IsNullOrEmpty(customer.Username) ? "" : customer.Username.ToLower())
                .Set(x => x.Deleted, customer.Deleted);

            await _customerRepository.Collection.UpdateOneAsync(filter, update);

            //event notification
            await _mediator.EntityUpdated(customer);
        }
        /// <summary>
        /// Updates the customer - last activity date
        /// </summary>
        /// <param name="customer">Customer</param>
        public virtual async Task UpdateCustomerLastActivityDate(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var builder = Builders<Customer>.Filter;
            var filter = builder.Eq(x => x.Id, customer.Id);
            var update = Builders<Customer>.Update
                .Set(x => x.LastActivityDateUtc, customer.LastActivityDateUtc);
            await _customerRepository.Collection.UpdateOneAsync(filter, update);

        }
        /// <summary>
        /// Updates the customer - last activity date
        /// </summary>
        /// <param name="customer">Customer</param>
        public virtual async Task UpdateCustomerLastLoginDate(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var builder = Builders<Customer>.Filter;
            var filter = builder.Eq(x => x.Id, customer.Id);
            var update = Builders<Customer>.Update
                .Set(x => x.LastLoginDateUtc, customer.LastLoginDateUtc)
                .Set(x => x.FailedLoginAttempts, customer.FailedLoginAttempts)
                .Set(x => x.CannotLoginUntilDateUtc, customer.CannotLoginUntilDateUtc);

            await _customerRepository.Collection.UpdateOneAsync(filter, update);

        }
        
        /// <summary>
        /// Updates the customer - last activity date
        /// </summary>
        /// <param name="customer">Customer</param>
        public virtual async Task UpdateCustomerLastIpAddress(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var builder = Builders<Customer>.Filter;
            var filter = builder.Eq(x => x.Id, customer.Id);
            var update = Builders<Customer>.Update
                .Set(x => x.LastIpAddress, customer.LastIpAddress);

            await _customerRepository.Collection.UpdateOneAsync(filter, update);
        }
        /// <summary>
        /// Updates the customer - password
        /// </summary>
        /// <param name="customer">Customer</param>
        public virtual async Task UpdateCustomerPassword(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var builder = Builders<Customer>.Filter;
            var filter = builder.Eq(x => x.Id, customer.Id);
            var update = Builders<Customer>.Update
                .Set(x => x.Password, customer.Password);

            await _customerRepository.Collection.UpdateOneAsync(filter, update);
        }
        public virtual async Task UpdateCustomerinAdminPanel(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var builder = Builders<Customer>.Filter;
            var filter = builder.Eq(x => x.Id, customer.Id);
            var update = Builders<Customer>.Update
                .Set(x => x.Active, customer.Active)
                .Set(x => x.AdminComment, customer.AdminComment)
                .Set(x => x.IsSystemAccount, customer.IsSystemAccount)
                .Set(x => x.Active, customer.Active)
                .Set(x => x.Email, string.IsNullOrEmpty(customer.Email) ? "" : customer.Email.ToLower())
                .Set(x => x.IsTaxExempt, customer.IsTaxExempt)
                .Set(x => x.Password, customer.Password)
                .Set(x => x.SystemName, customer.SystemName)
                .Set(x => x.Username, string.IsNullOrEmpty(customer.Username) ? "" : customer.Username.ToLower())
                .Set(x => x.CustomerRoles, customer.CustomerRoles)
                .Set(x => x.FreeShipping, customer.FreeShipping)
                .Set(x => x.StaffStoreId, customer.StaffStoreId);

            await _customerRepository.Collection.UpdateOneAsync(filter, update);
            //event notification
            await _mediator.EntityUpdated(customer);

        }
        public virtual async Task UpdateFreeShipping(string customerId, bool freeShipping)
        {
            var builder = Builders<Customer>.Filter;
            var filter = builder.Eq(x => x.Id, customerId);
            var update = Builders<Customer>.Update
                .Set(x => x.FreeShipping, freeShipping);

            await _customerRepository.Collection.UpdateOneAsync(filter, update);
        }

        public virtual async Task UpdateActive(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");
            var builder = Builders<Customer>.Filter;
            var filter = builder.Eq(x => x.Id, customer.Id);
            var update = Builders<Customer>.Update
                .Set(x => x.Active, customer.Active);

            await _customerRepository.Collection.UpdateOneAsync(filter, update);
        }

        public virtual async Task UpdateContributions(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");
            var builder = Builders<Customer>.Filter;
            var filter = builder.Eq(x => x.Id, customer.Id);
            var update = Builders<Customer>.Update
                .Set(x => x.HasContributions, true);
            await _customerRepository.Collection.UpdateOneAsync(filter, update);
        }

        public virtual async Task UpdateCustomerLastPurchaseDate(string customerId, DateTime date)
        {
            var builder = Builders<Customer>.Filter;
            var filter = builder.Eq(x => x.Id, customerId);
            var update = Builders<Customer>.Update
                .Set(x => x.LastPurchaseDateUtc, date);
            await _customerRepository.Collection.UpdateOneAsync(filter, update);
        }

        public virtual async Task UpdateCustomerLastUpdateCartDate(string customerId, DateTime? date)
        {
            var builder = Builders<Customer>.Filter;
            var filter = builder.Eq(x => x.Id, customerId);
            var update = Builders<Customer>.Update
                .Set(x => x.LastUpdateCartDateUtc, date);
            await _customerRepository.Collection.UpdateOneAsync(filter, update);
        }

        public virtual async Task UpdateCustomerLastUpdateWishList(string customerId, DateTime date)
        {
            var builder = Builders<Customer>.Filter;
            var filter = builder.Eq(x => x.Id, customerId);
            var update = Builders<Customer>.Update
                .Set(x => x.LastUpdateWishListDateUtc, date);
            await _customerRepository.Collection.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Delete guest customer records
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="onlyWithoutShoppingCart">A value indicating whether to delete customers only without shopping cart</param>
        /// <returns>Number of deleted customers</returns>
        public virtual async Task<int> DeleteGuestCustomers(DateTime? createdFromUtc, DateTime? createdToUtc)
        {

            var guestRole = await GetCustomerRoleBySystemName(SystemCustomerRoleNames.Guests);
            if (guestRole == null)
                throw new ForeverNoteException("'Guests' role could not be loaded");

            var builder = Builders<Customer>.Filter;
            var filter = builder.ElemMatch(x => x.CustomerRoles, role => role.Id == guestRole.Id);

            if (createdFromUtc.HasValue)
                filter = filter & builder.Gte(x => x.LastActivityDateUtc, createdFromUtc.Value);
            if (createdToUtc.HasValue)
                filter = filter & builder.Lte(x => x.LastActivityDateUtc, createdToUtc.Value);

            filter = filter & builder.Eq(x => x.HasContributions, false);

            filter = filter & builder.Eq(x => x.IsSystemAccount, false);

            var customers = await _customerRepository.Collection.DeleteManyAsync(filter);

            return (int)customers.DeletedCount;

        }

        #endregion

        #region Customer roles

        /// <summary>
        /// Delete a customer role
        /// </summary>
        /// <param name="customerRole">Customer role</param>
        public virtual async Task DeleteCustomerRole(CustomerRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException("customerRole");

            if (customerRole.IsSystemRole)
                throw new ForeverNoteException("System role could not be deleted");

            await _customerRoleRepository.DeleteAsync(customerRole);

            var builder = Builders<Customer>.Update;
            var updatefilter = builder.PullFilter(x => x.CustomerRoles, y => y.Id == customerRole.Id);
            await _customerRepository.Collection.UpdateManyAsync(new BsonDocument(), updatefilter);

            await _cacheManager.RemoveByPrefix(CUSTOMERROLES_PATTERN_KEY);

            //event notification
            await _mediator.EntityDeleted(customerRole);
        }

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="customerRoleId">Customer role identifier</param>
        /// <returns>Customer role</returns>
        public virtual Task<CustomerRole> GetCustomerRoleById(string customerRoleId)
        {
            if (string.IsNullOrWhiteSpace(customerRoleId))
                return Task.FromResult<CustomerRole>(null);

            return _customerRoleRepository.GetByIdAsync(customerRoleId);
        }

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="systemName">Customer role system name</param>
        /// <returns>Customer role</returns>
        public virtual Task<CustomerRole> GetCustomerRoleBySystemName(string systemName)
        {
            string key = string.Format(CUSTOMERROLES_BY_SYSTEMNAME_KEY, systemName);
            return _cacheManager.GetAsync(key, () =>
            {
                var filter = Builders<CustomerRole>.Filter.Eq(x => x.SystemName, systemName);
                return _customerRoleRepository.Collection.Find(filter).FirstOrDefaultAsync();
            });
        }

        /// <summary>
        /// Gets all customer roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer roles</returns>
        public virtual async Task<IPagedList<CustomerRole>> GetAllCustomerRoles(int pageIndex = 0, 
            int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = from cr in _customerRoleRepository.Table
                        where (showHidden || cr.Active)
                        orderby cr.Name
                        select cr;
            return await PagedList<CustomerRole>.Create(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Inserts a customer role
        /// </summary>
        /// <param name="customerRole">Customer role</param>
        public virtual async Task InsertCustomerRole(CustomerRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException("customerRole");

            await _customerRoleRepository.InsertAsync(customerRole);

            await _cacheManager.RemoveByPrefix(CUSTOMERROLES_PATTERN_KEY);

            //event notification
            await _mediator.EntityInserted(customerRole);
        }

        /// <summary>
        /// Updates the customer role
        /// </summary>
        /// <param name="customerRole">Customer role</param>
        public virtual async Task UpdateCustomerRole(CustomerRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException("customerRole");

            await _customerRoleRepository.UpdateAsync(customerRole);

            var builder = Builders<Customer>.Filter;
            var filter = builder.ElemMatch(x => x.CustomerRoles, y => y.Id == customerRole.Id);
            var update = Builders<Customer>.Update
                .Set(x => x.CustomerRoles.ElementAt(-1), customerRole);

            await _customerRepository.Collection.UpdateManyAsync(filter, update);

            await _cacheManager.RemoveByPrefix(CUSTOMERROLES_PATTERN_KEY);

            //event notification
            await _mediator.EntityUpdated(customerRole);
        }

        #endregion

        #region Customer role in customer

        public virtual async Task DeleteCustomerRoleInCustomer(CustomerRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException("pwi");

            var updatebuilder = Builders<Customer>.Update;
            var update = updatebuilder.Pull(p => p.CustomerRoles, customerRole);
            await _customerRepository.Collection.UpdateOneAsync(new BsonDocument("_id", customerRole.CustomerId), update);

        }

        public virtual async Task InsertCustomerRoleInCustomer(CustomerRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException("productWarehouse");

            var updatebuilder = Builders<Customer>.Update;
            var update = updatebuilder.AddToSet(p => p.CustomerRoles, customerRole);
            await _customerRepository.Collection.UpdateOneAsync(new BsonDocument("_id", customerRole.CustomerId), update);

        }

        #endregion

        #region Customer Role Products

        /// <summary>
        /// Delete a customer role product
        /// </summary>
        /// <param name="customerRoleProduct">Customer role product</param>
        public virtual async Task DeleteCustomerRoleProduct(CustomerRoleProduct customerRoleProduct)
        {
            if (customerRoleProduct == null)
                throw new ArgumentNullException("customerRole");

            await _customerRoleProductRepository.DeleteAsync(customerRoleProduct);

            //clear cache
            await _cacheManager.RemoveAsync(string.Format(CUSTOMERROLESPRODUCTS_ROLE_KEY, customerRoleProduct.CustomerRoleId));
            await _cacheManager.RemoveByPrefix(CUSTOMERROLESPRODUCTS_PATTERN_KEY);

            //event notification
            await _mediator.EntityDeleted(customerRoleProduct);
        }


        /// <summary>
        /// Inserts a customer role product
        /// </summary>
        /// <param name="customerRoleProduct">Customer role product</param>
        public virtual async Task InsertCustomerRoleProduct(CustomerRoleProduct customerRoleProduct)
        {
            if (customerRoleProduct == null)
                throw new ArgumentNullException("customerRoleProduct");

            await _customerRoleProductRepository.InsertAsync(customerRoleProduct);

            //clear cache
            await _cacheManager.RemoveAsync(string.Format(CUSTOMERROLESPRODUCTS_ROLE_KEY, customerRoleProduct.CustomerRoleId));
            await _cacheManager.RemoveByPrefix(CUSTOMERROLESPRODUCTS_PATTERN_KEY);

            //event notification
            await _mediator.EntityInserted(customerRoleProduct);
        }

        /// <summary>
        /// Updates the customer role product
        /// </summary>
        /// <param name="customerRoleProduct">Customer role product</param>
        public virtual async Task UpdateCustomerRoleProduct(CustomerRoleProduct customerRoleProduct)
        {
            if (customerRoleProduct == null)
                throw new ArgumentNullException("customerRoleProduct");

            var builder = Builders<CustomerRoleProduct>.Filter;
            var filter = builder.Eq(x => x.Id, customerRoleProduct.Id);
            var update = Builders<CustomerRoleProduct>.Update
                .Set(x => x.DisplayOrder, customerRoleProduct.DisplayOrder);
            await _customerRoleProductRepository.Collection.UpdateOneAsync(filter, update);

            //clear cache
            await _cacheManager.RemoveAsync(string.Format(CUSTOMERROLESPRODUCTS_ROLE_KEY, customerRoleProduct.CustomerRoleId));
            await _cacheManager.RemoveByPrefix(CUSTOMERROLESPRODUCTS_PATTERN_KEY);

            //event notification
            await _mediator.EntityUpdated(customerRoleProduct);
        }


        /// <summary>
        /// Gets customer roles products for customer role
        /// </summary>
        /// <param name="customerRoleId">Customer role id</param>
        /// <returns>Customer role products</returns>
        public virtual async Task<IList<CustomerRoleProduct>> GetCustomerRoleProducts(string customerRoleId)
        {
            string key = string.Format(CUSTOMERROLESPRODUCTS_ROLE_KEY, customerRoleId);
            return await _cacheManager.GetAsync(key, () =>
            {
                var filter = Builders<CustomerRoleProduct>.Filter.Eq(x => x.CustomerRoleId, customerRoleId);
                return _customerRoleProductRepository.Collection.Find(filter).SortBy(x => x.DisplayOrder).ToListAsync();
            });
        }

        /// <summary>
        /// Gets customer roles products for customer role
        /// </summary>
        /// <param name="customerRoleId">Customer role id</param>
        /// <param name="productId">Product id</param>
        /// <returns>Customer role product</returns>
        public virtual Task<CustomerRoleProduct> GetCustomerRoleProduct(string customerRoleId, string productId)
        {
            var filters = Builders<CustomerRoleProduct>.Filter;
            var filter = filters.Eq(x => x.CustomerRoleId, customerRoleId);
            filter = filter & filters.Eq(x => x.ProductId, productId);

            return _customerRoleProductRepository.Collection.Find(filter).SortBy(x => x.DisplayOrder).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets customer roles product
        /// </summary>
        /// <param name="Id">id</param>
        /// <returns>Customer role product</returns>
        public virtual Task<CustomerRoleProduct> GetCustomerRoleProductById(string id)
        {
            var query = from cr in _customerRoleProductRepository.Table
                        where cr.Id == id
                        orderby cr.DisplayOrder
                        select cr;

            return query.FirstOrDefaultAsync();
        }


        #endregion

        #region Customer note

        // <summary>
        /// Get note for customer
        /// </summary>
        /// <param name="id">Note identifier</param>
        /// <returns>CustomerNote</returns>
        public virtual Task<CustomerNote> GetCustomerNote(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Task.FromResult<CustomerNote>(null);

            return _customerNoteRepository.Table.Where(x => x.Id == id).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Deletes an customer note
        /// </summary>
        /// <param name="customerNote">The customer note</param>
        public virtual async Task DeleteCustomerNote(CustomerNote customerNote)
        {
            if (customerNote == null)
                throw new ArgumentNullException("customerNote");

            await _customerNoteRepository.DeleteAsync(customerNote);

            //event notification
            await _mediator.EntityDeleted(customerNote);
        }

        /// <summary>
        /// Insert an customer note
        /// </summary>
        /// <param name="customerNote">The customer note</param>
        public virtual async Task InsertCustomerNote(CustomerNote customerNote)
        {
            if (customerNote == null)
                throw new ArgumentNullException("customerNote");

            await _customerNoteRepository.InsertAsync(customerNote);

            //event notification
            await _mediator.EntityInserted(customerNote);
        }

        /// <summary>
        /// Get notes for customer
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="displaytocustomer">Display to customer</param>
        /// <returns>OrderNote</returns>
        public virtual async Task<IList<CustomerNote>> GetCustomerNotes(string customerId, bool? displaytocustomer = null)
        {
            var query = from customerNote in _customerNoteRepository.Table
                        where customerNote.CustomerId == customerId
                        select customerNote;

            if (displaytocustomer.HasValue)
                query = query.Where(x => x.DisplayToCustomer == displaytocustomer.Value);

            query = query.OrderByDescending(x => x.CreatedOnUtc);

            return await query.ToListAsync();
        }

        #endregion

        #endregion
    }
}