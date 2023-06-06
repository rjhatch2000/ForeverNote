using ForeverNote.Core.Caching;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Services.Commands.Models.Users;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    public partial class UserActionEventService : IUserActionEventService
    {
        #region Fields
        private const string CUSTOMER_ACTION_TYPE = "ForeverNote.user.action.type";

        private readonly IRepository<UserAction> _userActionRepository;
        private readonly IRepository<UserActionHistory> _userActionHistoryRepository;
        private readonly IRepository<UserActionType> _userActionTypeRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMediator _mediator;
        #endregion

        #region Ctor

        public UserActionEventService(
            IRepository<UserAction> userActionRepository,
            IRepository<UserActionType> userActionTypeRepository,
            IRepository<UserActionHistory> userActionHistoryRepository,
            ICacheManager cacheManager,
            IMediator mediator)
        {
            _userActionRepository = userActionRepository;
            _userActionTypeRepository = userActionTypeRepository;
            _userActionHistoryRepository = userActionHistoryRepository;
            _cacheManager = cacheManager;
            _mediator = mediator;
        }

        #endregion

        #region Utilities

        protected async Task<IList<UserActionType>> GetAllUserActionType()
        {
            return await _cacheManager.GetAsync(CUSTOMER_ACTION_TYPE, () =>
            {
                return _userActionTypeRepository.Table.ToListAsync();
            });
        }

        protected bool UsedAction(string actionId, string userId)
        {
            var query = from u in _userActionHistoryRepository.Table
                        where u.UserId == userId && u.UserActionId == actionId
                        select u.Id;
            if (query.Count() > 0)
                return true;

            return false;
        }

        #endregion

        #region Methods

        public virtual async Task Url(User user, string currentUrl, string previousUrl)
        {
            if (!user.IsSystemAccount)
            {
                var actiontypes = await GetAllUserActionType();
                var actionType = actiontypes.FirstOrDefault(x => x.SystemKeyword == UserActionTypeEnum.Url.ToString());
                if (actionType?.Enabled == true)
                {
                    var datetimeUtcNow = DateTime.UtcNow;
                    var query = from a in _userActionRepository.Table
                                where a.Active == true && a.ActionTypeId == actionType.Id
                                        && datetimeUtcNow >= a.StartDateTimeUtc && datetimeUtcNow <= a.EndDateTimeUtc
                                select a;

                    foreach (var item in query.ToList())
                    {
                        if (!UsedAction(item.Id, user.Id))
                        {
                            if (await _mediator.Send(new UserActionEventConditionCommand() {
                                UserActionTypes = actiontypes,
                                Action = item,
                                UserId = user.Id,
                                CurrentUrl = currentUrl,
                                PreviousUrl = previousUrl
                            }))
                            {
                                await _mediator.Send(new UserActionEventReactionCommand() {
                                    UserActionTypes = actiontypes,
                                    Action = item,
                                    UserId = user.Id
                                });
                            }
                        }
                    }
                }
            }
        }

        public virtual async Task Viewed(User user, string currentUrl, string previousUrl)
        {
            if (!user.IsSystemAccount)
            {
                var actiontypes = await GetAllUserActionType();
                var actionType = actiontypes.Where(x => x.SystemKeyword == UserActionTypeEnum.Viewed.ToString()).FirstOrDefault();
                if (actionType?.Enabled == true)
                {
                    var datetimeUtcNow = DateTime.UtcNow;
                    var query = from a in _userActionRepository.Table
                                where a.Active == true && a.ActionTypeId == actionType.Id
                                        && datetimeUtcNow >= a.StartDateTimeUtc && datetimeUtcNow <= a.EndDateTimeUtc
                                select a;

                    foreach (var item in query.ToList())
                    {
                        if (!UsedAction(item.Id, user.Id))
                        {
                            if (await _mediator.Send(new UserActionEventConditionCommand() {
                                UserActionTypes = actiontypes,
                                Action = item,
                                UserId = user.Id,
                                CurrentUrl = currentUrl,
                                PreviousUrl = previousUrl
                            }))
                            {
                                await _mediator.Send(new UserActionEventReactionCommand() {
                                    UserActionTypes = actiontypes,
                                    Action = item,
                                    UserId = user.Id
                                });
                            }
                        }
                    }

                }
            }

        }

        public virtual async Task Registration(User user)
        {
            var actiontypes = await GetAllUserActionType();
            var actionType = actiontypes.Where(x => x.SystemKeyword == UserActionTypeEnum.Registration.ToString()).FirstOrDefault();
            if (actionType?.Enabled == true)
            {
                var datetimeUtcNow = DateTime.UtcNow;
                var query = from a in _userActionRepository.Table
                            where a.Active == true && a.ActionTypeId == actionType.Id
                                    && datetimeUtcNow >= a.StartDateTimeUtc && datetimeUtcNow <= a.EndDateTimeUtc
                            select a;

                foreach (var item in query.ToList())
                {
                    if (!UsedAction(item.Id, user.Id))
                    {
                        if (await _mediator.Send(new UserActionEventConditionCommand() {
                            UserActionTypes = actiontypes,
                            Action = item,
                            UserId = user.Id
                        }))
                        {
                            await _mediator.Send(new UserActionEventReactionCommand() {
                                UserActionTypes = actiontypes,
                                Action = item,
                                UserId = user.Id
                            });
                        }
                    }
                }

            }
        }

        #endregion
    }
}
