using ForeverNote.Core.Caching;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Services.Events;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    /// <summary>
    /// User attribute service
    /// </summary>
    public partial class UserAttributeService : IUserAttributeService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string CUSTOMERATTRIBUTES_ALL_KEY = "ForeverNote.userattribute.all";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user attribute ID
        /// </remarks>
        private const string CUSTOMERATTRIBUTES_BY_ID_KEY = "ForeverNote.userattribute.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CUSTOMERATTRIBUTES_PATTERN_KEY = "ForeverNote.userattribute.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CUSTOMERATTRIBUTEVALUES_PATTERN_KEY = "ForeverNote.userattributevalue.";
        #endregion
        
        #region Fields

        private readonly IRepository<UserAttribute> _userAttributeRepository;
        private readonly IMediator _mediator;
        private readonly ICacheManager _cacheManager;
        
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="userAttributeRepository">User attribute repository</param>
        /// <param name="mediator">Mediator</param>
        public UserAttributeService(ICacheManager cacheManager,
            IRepository<UserAttribute> userAttributeRepository,            
            IMediator mediator)
        {
            _cacheManager = cacheManager;
            _userAttributeRepository = userAttributeRepository;
            _mediator = mediator;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a user attribute
        /// </summary>
        /// <param name="userAttribute">User attribute</param>
        public virtual async Task DeleteUserAttribute(UserAttribute userAttribute)
        {
            if (userAttribute == null)
                throw new ArgumentNullException("userAttribute");

            await _userAttributeRepository.DeleteAsync(userAttribute);

            await _cacheManager.RemoveByPrefix(CUSTOMERATTRIBUTES_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(CUSTOMERATTRIBUTEVALUES_PATTERN_KEY);

            //event notification
            await _mediator.EntityDeleted(userAttribute);
        }

        /// <summary>
        /// Gets all user attributes
        /// </summary>
        /// <returns>User attributes</returns>
        public virtual async Task<IList<UserAttribute>> GetAllUserAttributes()
        {
            string key = CUSTOMERATTRIBUTES_ALL_KEY;
            return await _cacheManager.GetAsync(key, () =>
            {
                var query = from ca in _userAttributeRepository.Table
                            orderby ca.DisplayOrder
                            select ca;
                return query.ToListAsync();
            });
        }

        /// <summary>
        /// Gets a user attribute 
        /// </summary>
        /// <param name="userAttributeId">User attribute identifier</param>
        /// <returns>User attribute</returns>
        public virtual Task<UserAttribute> GetUserAttributeById(string userAttributeId)
        {
            string key = string.Format(CUSTOMERATTRIBUTES_BY_ID_KEY, userAttributeId);
            return _cacheManager.GetAsync(key, () => _userAttributeRepository.GetByIdAsync(userAttributeId));
        }

        /// <summary>
        /// Inserts a user attribute
        /// </summary>
        /// <param name="userAttribute">User attribute</param>
        public virtual async Task InsertUserAttribute(UserAttribute userAttribute)
        {
            if (userAttribute == null)
                throw new ArgumentNullException("userAttribute");

            await _userAttributeRepository.InsertAsync(userAttribute);

            await _cacheManager.RemoveByPrefix(CUSTOMERATTRIBUTES_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(CUSTOMERATTRIBUTEVALUES_PATTERN_KEY);

            //event notification
            await _mediator.EntityInserted(userAttribute);
        }

        /// <summary>
        /// Updates the user attribute
        /// </summary>
        /// <param name="userAttribute">User attribute</param>
        public virtual async Task UpdateUserAttribute(UserAttribute userAttribute)
        {
            if (userAttribute == null)
                throw new ArgumentNullException("userAttribute");

            await _userAttributeRepository.UpdateAsync(userAttribute);

            await _cacheManager.RemoveByPrefix(CUSTOMERATTRIBUTES_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(CUSTOMERATTRIBUTEVALUES_PATTERN_KEY);

            //event notification
            await _mediator.EntityUpdated(userAttribute);
        }

        /// <summary>
        /// Deletes a user attribute value
        /// </summary>
        /// <param name="userAttributeValue">User attribute value</param>
        public virtual async Task DeleteUserAttributeValue(UserAttributeValue userAttributeValue)
        {
            if (userAttributeValue == null)
                throw new ArgumentNullException("userAttributeValue");

            var updatebuilder = Builders<UserAttribute>.Update;
            var update = updatebuilder.Pull(p => p.UserAttributeValues, userAttributeValue);
            await _userAttributeRepository.Collection.UpdateOneAsync(new BsonDocument("_id", userAttributeValue.UserAttributeId), update);

            await _cacheManager.RemoveByPrefix(CUSTOMERATTRIBUTES_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(CUSTOMERATTRIBUTEVALUES_PATTERN_KEY);

            //event notification
            await _mediator.EntityDeleted(userAttributeValue);
        }

        /// <summary>
        /// Inserts a user attribute value
        /// </summary>
        /// <param name="userAttributeValue">User attribute value</param>
        public virtual async Task InsertUserAttributeValue(UserAttributeValue userAttributeValue)
        {
            if (userAttributeValue == null)
                throw new ArgumentNullException("userAttributeValue");

            var updatebuilder = Builders<UserAttribute>.Update;
            var update = updatebuilder.AddToSet(p => p.UserAttributeValues, userAttributeValue);
            await _userAttributeRepository.Collection.UpdateOneAsync(new BsonDocument("_id", userAttributeValue.UserAttributeId), update);

            await _cacheManager.RemoveByPrefix(CUSTOMERATTRIBUTES_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(CUSTOMERATTRIBUTEVALUES_PATTERN_KEY);

            //event notification
            await _mediator.EntityInserted(userAttributeValue);
        }

        /// <summary>
        /// Updates the user attribute value
        /// </summary>
        /// <param name="userAttributeValue">User attribute value</param>
        public virtual async Task UpdateUserAttributeValue(UserAttributeValue userAttributeValue)
        {
            if (userAttributeValue == null)
                throw new ArgumentNullException("userAttributeValue");

            var builder = Builders<UserAttribute>.Filter;
            var filter = builder.Eq(x => x.Id, userAttributeValue.UserAttributeId);
            filter = filter & builder.ElemMatch(x => x.UserAttributeValues, y => y.Id == userAttributeValue.Id);
            var update = Builders<UserAttribute>.Update
                .Set(x => x.UserAttributeValues.ElementAt(-1).DisplayOrder, userAttributeValue.DisplayOrder)
                .Set(x => x.UserAttributeValues.ElementAt(-1).IsPreSelected, userAttributeValue.IsPreSelected)
                .Set(x => x.UserAttributeValues.ElementAt(-1).Locales, userAttributeValue.Locales)
                .Set(x => x.UserAttributeValues.ElementAt(-1).Name, userAttributeValue.Name);

            await _userAttributeRepository.Collection.UpdateManyAsync(filter, update);

            await _cacheManager.RemoveByPrefix(CUSTOMERATTRIBUTES_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(CUSTOMERATTRIBUTEVALUES_PATTERN_KEY);

            //event notification
            await _mediator.EntityUpdated(userAttributeValue);
        }
        
        #endregion
    }
}
