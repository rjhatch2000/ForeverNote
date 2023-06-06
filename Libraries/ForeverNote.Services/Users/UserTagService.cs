using ForeverNote.Core;
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
    /// User tag service
    /// </summary>
    public partial class UserTagService : IUserTagService
    {
        #region Fields

        private readonly IRepository<UserTag> _userTagRepository;
        private readonly IRepository<UserTagNote> _userTagNoteRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMediator _mediator;
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user tag Id?
        /// </remarks>
        private const string CUSTOMERTAGPRODUCTS_ROLE_KEY = "ForeverNote.usertagnotes.tag-{0}";

        private const string PRODUCTS_CUSTOMER_TAG = "ForeverNote.note.ct";

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public UserTagService(IRepository<UserTag> userTagRepository,
            IRepository<UserTagNote> userTagNoteRepository,
            IRepository<User> userRepository,
            IMediator mediator,
            ICacheManager cacheManager
            )
        {
            _userTagRepository = userTagRepository;
            _userTagNoteRepository = userTagNoteRepository;
            _mediator = mediator;
            _userRepository = userRepository;
            _cacheManager = cacheManager;
        }

        #endregion

        /// <summary>
        /// Gets all user for tag id
        /// </summary>
        /// <returns>Users</returns>
        public virtual async Task<IPagedList<User>> GetUsersByTag(string userTagId = "", int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = from c in _userRepository.Table
                        where c.UserTags.Contains(userTagId)
                        select c;
            return await PagedList<User>.Create(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Delete a user tag
        /// </summary>
        /// <param name="userTag">User tag</param>
        public virtual async Task DeleteUserTag(UserTag userTag)
        {
            if (userTag == null)
                throw new ArgumentNullException("noteTag");

            var builder = Builders<User>.Update;
            var updatefilter = builder.Pull(x => x.UserTags, userTag.Id);
            await _userRepository.Collection.UpdateManyAsync(new BsonDocument(), updatefilter);

            await _userTagRepository.DeleteAsync(userTag);

            //event notification
            await _mediator.EntityDeleted(userTag);
        }

        /// <summary>
        /// Gets all user tags
        /// </summary>
        /// <returns>User tags</returns>
        public virtual async Task<IList<UserTag>> GetAllUserTags()
        {
            var query = _userTagRepository.Table;
            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets user tag
        /// </summary>
        /// <param name="userTagId">User tag identifier</param>
        /// <returns>User tag</returns>
        public virtual Task<UserTag> GetUserTagById(string userTagId)
        {
            return _userTagRepository.GetByIdAsync(userTagId);
        }

        /// <summary>
        /// Gets user tag by name
        /// </summary>
        /// <param name="name">User tag name</param>
        /// <returns>User tag</returns>
        public virtual Task<UserTag> GetUserTagByName(string name)
        {
            var query = from pt in _userTagRepository.Table
                        where pt.Name == name
                        select pt;

            return query.FirstOrDefaultAsync(); 
        }

        /// <summary>
        /// Gets user tags search by name
        /// </summary>
        /// <param name="name">User tags name</param>
        /// <returns>User tags</returns>
        public virtual async Task<IList<UserTag>> GetUserTagsByName(string name)
        {
            var query = from pt in _userTagRepository.Table
                        where pt.Name.ToLower().Contains(name.ToLower())
                        select pt;
            return await query.ToListAsync();
        }

        /// <summary>
        /// Inserts a user tag
        /// </summary>
        /// <param name="userTag">User tag</param>
        public virtual async Task InsertUserTag(UserTag userTag)
        {
            if (userTag == null)
                throw new ArgumentNullException("userTag");

            await _userTagRepository.InsertAsync(userTag);

            //event notification
            await _mediator.EntityInserted(userTag);
        }

        /// <summary>
        /// Insert tag to a user
        /// </summary>
        public virtual async Task InsertTagToUser(string userTagId, string userId)
        {
            var updatebuilder = Builders<User>.Update;
            var update = updatebuilder.AddToSet(p => p.UserTags, userTagId);
            await _userRepository.Collection.UpdateOneAsync(new BsonDocument("_id", userId), update);
        }

        /// <summary>
        /// Delete tag from a user
        /// </summary>
        public virtual async Task DeleteTagFromUser(string userTagId, string userId)
        {
            var updatebuilder = Builders<User>.Update;
            var update = updatebuilder.Pull(p => p.UserTags, userTagId);
            await _userRepository.Collection.UpdateOneAsync(new BsonDocument("_id", userId), update);
        }

        /// <summary>
        /// Updates the user tag
        /// </summary>
        /// <param name="userTag">User tag</param>
        public virtual async Task UpdateUserTag(UserTag userTag)
        {
            if (userTag == null)
                throw new ArgumentNullException("userTag");

            await _userTagRepository.UpdateAsync(userTag);

            //event notification
            await _mediator.EntityUpdated(userTag);
        }

        /// <summary>
        /// Get number of users
        /// </summary>
        /// <param name="userTagId">User tag identifier</param>
        /// <returns>Number of users</returns>
        public virtual async Task<int> GetUserCount(string userTagId)
        {
            var query = await _userRepository.Table.
                Where(x => x.UserTags.Contains(userTagId)).
                GroupBy(p => p, (k, s) => new { Counter = s.Count() }).ToListAsync();
            if(query.Count > 0)
                return query.FirstOrDefault().Counter;
            return 0;
        }

        #region User tag note


        /// <summary>
        /// Gets user tag notes for user tag
        /// </summary>
        /// <param name="userTagId">User tag id</param>
        /// <returns>User tag notes</returns>
        public virtual async Task<IList<UserTagNote>> GetUserTagNotes(string userTagId)
        {
            string key = string.Format(CUSTOMERTAGPRODUCTS_ROLE_KEY, userTagId);
            return await _cacheManager.GetAsync(key, () =>
            {
                var query = from cr in _userTagNoteRepository.Table
                            where (cr.UserTagId == userTagId)
                            orderby cr.DisplayOrder
                            select cr;
                return query.ToListAsync();
            });
        }

        /// <summary>
        /// Gets user tag notes for user tag
        /// </summary>
        /// <param name="userTagId">User tag id</param>
        /// <param name="noteId">Note id</param>
        /// <returns>User tag note</returns>
        public virtual Task<UserTagNote> GetUserTagNote(string userTagId, string noteId)
        {
            var query = from cr in _userTagNoteRepository.Table
                        where cr.UserTagId == userTagId && cr.NoteId == noteId
                        orderby cr.DisplayOrder
                        select cr;
            return query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets user tag note
        /// </summary>
        /// <param name="Id">id</param>
        /// <returns>User tag note</returns>
        public virtual Task<UserTagNote> GetUserTagNoteById(string id)
        {
            return _userTagNoteRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Inserts a user tag note
        /// </summary>
        /// <param name="userTagNote">User tag note</param>
        public virtual async Task InsertUserTagNote(UserTagNote userTagNote)
        {
            if (userTagNote == null)
                throw new ArgumentNullException("userTagNote");

            await _userTagNoteRepository.InsertAsync(userTagNote);

            //clear cache
            await _cacheManager.RemoveAsync(string.Format(CUSTOMERTAGPRODUCTS_ROLE_KEY, userTagNote.UserTagId));
            await _cacheManager.RemoveByPrefix(PRODUCTS_CUSTOMER_TAG);

            //event notification
            await _mediator.EntityInserted(userTagNote);
        }

        /// <summary>
        /// Updates the user tag note
        /// </summary>
        /// <param name="userTagNote">User tag note</param>
        public virtual async Task UpdateUserTagNote(UserTagNote userTagNote)
        {
            if (userTagNote == null)
                throw new ArgumentNullException("userTagNote");

            await _userTagNoteRepository.UpdateAsync(userTagNote);

            //clear cache
            await _cacheManager.RemoveAsync(string.Format(CUSTOMERTAGPRODUCTS_ROLE_KEY, userTagNote.UserTagId));
            await _cacheManager.RemoveByPrefix(PRODUCTS_CUSTOMER_TAG);

            //event notification
            await _mediator.EntityUpdated(userTagNote);
        }

        /// <summary>
        /// Delete a user tag note
        /// </summary>
        /// <param name="userTagNote">User tag note</param>
        public virtual async Task DeleteUserTagNote(UserTagNote userTagNote)
        {
            if (userTagNote == null)
                throw new ArgumentNullException("userTagNote");

            await _userTagNoteRepository.DeleteAsync(userTagNote);

            //clear cache
            await _cacheManager.RemoveAsync(string.Format(CUSTOMERTAGPRODUCTS_ROLE_KEY, userTagNote.UserTagId));
            await _cacheManager.RemoveByPrefix(PRODUCTS_CUSTOMER_TAG);
            //event notification
            await _mediator.EntityDeleted(userTagNote);
        }

        #endregion

    }
}
