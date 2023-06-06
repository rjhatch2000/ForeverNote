using ForeverNote.Core;
using ForeverNote.Core.Caching;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Users;
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
    public partial class UserActionService : IUserActionService
    {
        #region Fields

        private const string CUSTOMER_ACTION_TYPE = "ForeverNote.user.action.type";

        private readonly IRepository<UserAction> _userActionRepository;
        private readonly IRepository<UserActionType> _userActionTypeRepository;
        private readonly IRepository<UserActionHistory> _userActionHistoryRepository;
        private readonly IMediator _mediator;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        public UserActionService(IRepository<UserAction> userActionRepository,
            IRepository<UserActionType> userActionTypeRepository,
            IRepository<UserActionHistory> userActionHistoryRepository,
            IMediator mediator,
            ICacheManager cacheManager)
        {
            _userActionRepository = userActionRepository;
            _userActionTypeRepository = userActionTypeRepository;
            _userActionHistoryRepository = userActionHistoryRepository;
            _mediator = mediator;
            _cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets user action
        /// </summary>
        /// <param name="id">User action identifier</param>
        /// <returns>User Action</returns>
        public virtual Task<UserAction> GetUserActionById(string id)
        {
            return _userActionRepository.GetByIdAsync(id);
        }


        /// <summary>
        /// Gets all user actions
        /// </summary>
        /// <returns>User actions</returns>
        public virtual async Task<IList<UserAction>> GetUserActions()
        {
            var query = _userActionRepository.Table;
            return await query.ToListAsync();
        }

        /// <summary>
        /// Inserts a user action
        /// </summary>
        /// <param name="UserAction">User action</param>
        public virtual async Task InsertUserAction(UserAction userAction)
        {
            if (userAction == null)
                throw new ArgumentNullException("userAction");

            await _userActionRepository.InsertAsync(userAction);

            //event notification
            await _mediator.EntityInserted(userAction);

        }

        /// <summary>
        /// Delete a user action
        /// </summary>
        /// <param name="userAction">User action</param>
        public virtual async Task DeleteUserAction(UserAction userAction)
        {
            if (userAction == null)
                throw new ArgumentNullException("userAction");

            await _userActionRepository.DeleteAsync(userAction);

            //event notification
            await _mediator.EntityDeleted(userAction);

        }

        /// <summary>
        /// Updates the user action
        /// </summary>
        /// <param name="userTag">User tag</param>
        public virtual async Task UpdateUserAction(UserAction userAction)
        {
            if (userAction == null)
                throw new ArgumentNullException("userAction");

            await _userActionRepository.UpdateAsync(userAction);

            //event notification
            await _mediator.EntityUpdated(userAction);
        }

        #endregion

        #region Condition Type

        public virtual async Task<IList<UserActionType>> GetUserActionType()
        {
            var query = _userActionTypeRepository.Table;
            return await query.ToListAsync();
        }

        public virtual async Task<IPagedList<UserActionHistory>> GetAllUserActionHistory(string userActionId, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = from h in _userActionHistoryRepository.Table
                        where h.UserActionId == userActionId
                        select h;
            return await PagedList<UserActionHistory>.Create(query, pageIndex, pageSize);
        }

        public virtual async Task<UserActionType> GetUserActionTypeById(string id)
        {
            return await _userActionTypeRepository.GetByIdAsync(id);
        }

        public virtual async Task UpdateUserActionType(UserActionType userActionType)
        {
            if (userActionType == null)
                throw new ArgumentNullException("userActionType");

            await _userActionTypeRepository.UpdateAsync(userActionType);

            //clear cache
            await _cacheManager.RemoveAsync(CUSTOMER_ACTION_TYPE);
            //event notification
            await _mediator.EntityUpdated(userActionType);
        }

        #endregion

    }
}
