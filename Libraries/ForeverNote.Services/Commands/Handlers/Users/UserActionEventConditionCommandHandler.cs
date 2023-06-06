using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Logging;
using ForeverNote.Core.Domain.Notes;
using ForeverNote.Services.Commands.Models.Users;
using ForeverNote.Services.Users;
using ForeverNote.Services.Helpers;
using ForeverNote.Services.Notes;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Commands.Handlers.Users
{
    public class UserActionEventConditionCommandHandler : IRequestHandler<UserActionEventConditionCommand, bool>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INoteService _noteService;
        private readonly IUserService _userService;
        private readonly IRepository<ActivityLog> _activityLogRepository;
        private readonly IRepository<ActivityLogType> _activityLogTypeRepository;

        public UserActionEventConditionCommandHandler(
            IServiceProvider serviceProvider,
            INoteService noteService,
            IUserService userService,
            IRepository<ActivityLog> activityLogRepository,
            IRepository<ActivityLogType> activityLogTypeRepository)
        {
            _serviceProvider = serviceProvider;
            _noteService = noteService;
            _userService = userService;
            _activityLogRepository = activityLogRepository;
            _activityLogTypeRepository = activityLogTypeRepository;
        }

        public async Task<bool> Handle(UserActionEventConditionCommand request, CancellationToken cancellationToken)
        {
            return await Condition(
                request.UserActionTypes,
                request.Action,
                request.NoteId,
                request.AttributesXml,
                request.UserId,
                request.CurrentUrl,
                request.PreviousUrl);
        }

        protected async Task<bool> Condition(IList<UserActionType> userActionTypes, UserAction action, string noteId, string attributesXml, string userId, string currentUrl, string previousUrl)
        {
            if (!action.Conditions.Any())
                return true;

            bool cond = false;
            foreach (var item in action.Conditions)
            {
                #region note
                if (!string.IsNullOrEmpty(noteId))
                {
                    var note = await _noteService.GetNoteById(noteId);

                    if (item.UserActionConditionType == UserActionConditionTypeEnum.Notebook)
                    {
                        cond = ConditionNotebook(item, note.NoteNotebooks);
                    }

                    if (item.UserActionConditionType == UserActionConditionTypeEnum.Note)
                    {
                        cond = ConditionNotes(item, note.Id);
                    }
                }
                #endregion

                #region Action type viewed
                if (action.ActionTypeId == userActionTypes.FirstOrDefault(x => x.SystemKeyword == "Viewed").Id)
                {
                    cond = false;
                    if (item.UserActionConditionType == UserActionConditionTypeEnum.Notebook)
                    {
                        var _actLogType = (from a in _activityLogTypeRepository.Table
                                           where a.SystemKeyword == "PublicSite.ViewNotebook"
                                           select a).FirstOrDefault();
                        if (_actLogType != null)
                        {
                            if (_actLogType.Enabled)
                            {
                                var noteNotebook = (from p in _activityLogRepository.Table
                                                       where p.UserId == userId && p.ActivityLogTypeId == _actLogType.Id
                                                       select p.EntityKeyId).Distinct().ToList();
                                cond = ConditionNotebook(item, noteNotebook);
                            }
                        }
                    }

                    if (item.UserActionConditionType == UserActionConditionTypeEnum.Note)
                    {
                        cond = false;
                        var _actLogType = (from a in _activityLogTypeRepository.Table
                                           where a.SystemKeyword == "PublicSite.ViewNote"
                                           select a).FirstOrDefault();
                        if (_actLogType != null)
                        {
                            if (_actLogType.Enabled)
                            {
                                var notes = (from p in _activityLogRepository.Table
                                                where p.UserId == userId && p.ActivityLogTypeId == _actLogType.Id
                                                select p.EntityKeyId).Distinct().ToList();
                                cond = ConditionNotes(item, notes);
                            }
                        }
                    }
                }
                #endregion

                var user = await _userService.GetUserById(userId);

                if (item.UserActionConditionType == UserActionConditionTypeEnum.UserTag)
                {
                    cond = ConditionUserTag(item, user);
                }

                if (item.UserActionConditionType == UserActionConditionTypeEnum.UserRegisterField)
                {
                    cond = await ConditionUserRegister(item, user);
                }

                if (item.UserActionConditionType == UserActionConditionTypeEnum.CustomUserAttribute)
                {
                    cond = await ConditionUserAttribute(item, user);
                }

                if (item.UserActionConditionType == UserActionConditionTypeEnum.UrlCurrent)
                {
                    cond = item.UrlCurrent.Select(x => x.Name).Contains(currentUrl);
                }

                if (item.UserActionConditionType == UserActionConditionTypeEnum.UrlReferrer)
                {
                    cond = item.UrlReferrer.Select(x => x.Name).Contains(previousUrl);
                }

                if (action.Condition == UserActionConditionEnum.OneOfThem && cond)
                    return true;
                if (action.Condition == UserActionConditionEnum.AllOfThem && !cond)
                    return false;
            }

            return cond;
        }
        protected bool ConditionNotebook(UserAction.ActionCondition condition, ICollection<NoteNotebook> categorties)
        {
            bool cond = true;
            if (condition.Condition == UserActionConditionEnum.AllOfThem)
            {
                cond = categorties.Select(x => x.NotebookId).ContainsAll(condition.Notebooks);
            }
            if (condition.Condition == UserActionConditionEnum.OneOfThem)
            {
                cond = categorties.Select(x => x.NotebookId).ContainsAny(condition.Notebooks);
            }

            return cond;
        }
        protected bool ConditionNotebook(UserAction.ActionCondition condition, ICollection<string> categorties)
        {
            bool cond = true;
            if (condition.Condition == UserActionConditionEnum.AllOfThem)
            {
                cond = categorties.ContainsAll(condition.Notebooks);
            }
            if (condition.Condition == UserActionConditionEnum.OneOfThem)
            {
                cond = categorties.ContainsAny(condition.Notebooks);
            }

            return cond;
        }
        protected bool ConditionNotes(UserAction.ActionCondition condition, string noteId)
        {
            return condition.Notes.Contains(noteId);
        }
        protected bool ConditionNotes(UserAction.ActionCondition condition, ICollection<string> notes)
        {
            bool cond = true;
            if (condition.Condition == UserActionConditionEnum.AllOfThem)
            {
                cond = notes.ContainsAll(condition.Notes);
            }
            if (condition.Condition == UserActionConditionEnum.OneOfThem)
            {
                cond = notes.ContainsAny(condition.Notes);
            }

            return cond;
        }
        protected bool ConditionUserTag(UserAction.ActionCondition condition, User user)
        {
            bool cond = false;
            if (user != null)
            {
                var userTags = user.UserTags;
                if (condition.Condition == UserActionConditionEnum.AllOfThem)
                {
                    cond = userTags.Select(x => x).ContainsAll(condition.UserTags);
                }
                if (condition.Condition == UserActionConditionEnum.OneOfThem)
                {
                    cond = userTags.Select(x => x).ContainsAny(condition.UserTags);
                }
            }
            return cond;
        }
        protected async Task<bool> ConditionUserRegister(UserAction.ActionCondition condition, User user)
        {
            bool cond = false;
            if (user != null)
            {
                var _genericAttributes = user.GenericAttributes;
                if (condition.Condition == UserActionConditionEnum.AllOfThem)
                {
                    cond = true;
                    foreach (var item in condition.UserRegistration)
                    {
                        if (_genericAttributes.Where(x => x.Key == item.RegisterField && x.Value.ToLower() == item.RegisterValue.ToLower()).Count() == 0)
                            cond = false;
                    }
                }
                if (condition.Condition == UserActionConditionEnum.OneOfThem)
                {
                    foreach (var item in condition.UserRegistration)
                    {
                        if (_genericAttributes.Where(x => x.Key == item.RegisterField && x.Value.ToLower() == item.RegisterValue.ToLower()).Count() > 0)
                            cond = true;
                    }
                }
            }
            return await Task.FromResult(cond);
        }
        protected async Task<bool> ConditionUserAttribute(UserAction.ActionCondition condition, User user)
        {
            bool cond = false;
            if (user != null)
            {
                var userAttributeParser = _serviceProvider.GetRequiredService<IUserAttributeParser>();

                var _genericAttributes = user.GenericAttributes;
                if (condition.Condition == UserActionConditionEnum.AllOfThem)
                {
                    var customUserAttributes = _genericAttributes.FirstOrDefault(x => x.Key == "CustomUserAttributes");
                    if (customUserAttributes != null)
                    {
                        if (!String.IsNullOrEmpty(customUserAttributes.Value))
                        {
                            var selectedValues = await userAttributeParser.ParseUserAttributeValues(customUserAttributes.Value);
                            cond = true;
                            foreach (var item in condition.CustomUserAttributes)
                            {
                                var _fields = item.RegisterField.Split(':');
                                if (_fields.Count() > 1)
                                {
                                    if (selectedValues.Where(x => x.UserAttributeId == _fields.FirstOrDefault() && x.Id == _fields.LastOrDefault()).Count() == 0)
                                        cond = false;
                                }
                                else
                                    cond = false;
                            }
                        }
                    }
                }
                if (condition.Condition == UserActionConditionEnum.OneOfThem)
                {
                    var customUserAttributes = _genericAttributes.FirstOrDefault(x => x.Key == "CustomUserAttributes");
                    if (customUserAttributes != null)
                    {
                        if (!String.IsNullOrEmpty(customUserAttributes.Value))
                        {
                            var selectedValues = await userAttributeParser.ParseUserAttributeValues(customUserAttributes.Value);
                            foreach (var item in condition.CustomUserAttributes)
                            {
                                var _fields = item.RegisterField.Split(':');
                                if (_fields.Count() > 1)
                                {
                                    if (selectedValues.Where(x => x.UserAttributeId == _fields.FirstOrDefault() && x.Id == _fields.LastOrDefault()).Count() > 0)
                                        cond = true;
                                }
                            }
                        }
                    }
                }
            }
            return cond;
        }

    }
}
