using ForeverNote.Core;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Services.Common;
using ForeverNote.Services.Events;
using ForeverNote.Services.Helpers;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Logging;
using ForeverNote.Services.Messages;
using ForeverNote.Services.Messages.DotLiquidDrops;
using ForeverNote.Services.Notes;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Users
{
    public partial class UserReminderService : IUserReminderService
    {
        #region Fields

        private readonly IRepository<UserReminder> _userReminderRepository;
        private readonly IRepository<UserReminderHistory> _userReminderHistoryRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMediator _mediator;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IUserAttributeParser _userAttributeParser;
        private readonly INoteService _noteService;
        private readonly IUserActivityService _userActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;

        #endregion

        #region Ctor

        public UserReminderService(
            IRepository<UserReminder> userReminderRepository,
            IRepository<UserReminderHistory> userReminderHistoryRepository,
            IRepository<User> userRepository,
            IMediator mediator,
            IEmailAccountService emailAccountService,
            IQueuedEmailService queuedEmailService,
            IMessageTokenProvider messageTokenProvider,
            INoteService noteService,
            IUserAttributeParser userAttributeParser,
            IUserActivityService userActivityService,
            ILocalizationService localizationService,
            ILanguageService languageService
        )
        {
            _userReminderRepository = userReminderRepository;
            _userReminderHistoryRepository = userReminderHistoryRepository;
            _userRepository = userRepository;
            _mediator = mediator;
            _emailAccountService = emailAccountService;
            _messageTokenProvider = messageTokenProvider;
            _queuedEmailService = queuedEmailService;
            _userAttributeParser = userAttributeParser;
            _noteService = noteService;
            _userActivityService = userActivityService;
            _localizationService = localizationService;
            _languageService = languageService;
        }

        #endregion

        #region Utilities

        protected async Task<bool> SendEmail(User user, UserReminder userReminder, string reminderlevelId)
        {
            var reminderLevel = userReminder.Levels.FirstOrDefault(x => x.Id == reminderlevelId);
            var emailAccount = await _emailAccountService.GetEmailAccountById(reminderLevel.EmailAccountId);

            //retrieve message template data
            var bcc = reminderLevel.BccEmailAddresses;
            var languages = await _languageService.GetAllLanguages();
            var langId = user.GetAttributeFromEntity<string>(SystemUserAttributeNames.LanguageId);
            if (string.IsNullOrEmpty(langId))
                langId = languages.FirstOrDefault().Id;

            var language = languages.FirstOrDefault(x => x.Id == langId);
            if (language == null)
                language = languages.FirstOrDefault();

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddUserTokens(liquidObject, user, language);

            var body = LiquidExtensions.Render(liquidObject, reminderLevel.Body);
            var subject = LiquidExtensions.Render(liquidObject, reminderLevel.Subject);

            //limit name length
            var toName = CommonHelper.EnsureMaximumLength(user.GetFullName(), 300);
            var email = new QueuedEmail {
                Priority = QueuedEmailPriority.High,
                From = emailAccount.Email,
                FromName = emailAccount.DisplayName,
                To = user.Email,
                ToName = toName,
                ReplyTo = string.Empty,
                ReplyToName = string.Empty,
                CC = string.Empty,
                Bcc = bcc,
                Subject = subject,
                Body = body,
                AttachmentFilePath = "",
                AttachmentFileName = "",
                AttachedDownloads = null,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id,
            };

            await _queuedEmailService.InsertQueuedEmail(email);
            //activity log
            await _userActivityService.InsertActivity(string.Format("UserReminder.{0}", userReminder.ReminderRule.ToString()), user.Id, _localizationService.GetResource(string.Format("ActivityLog.{0}", userReminder.ReminderRule.ToString())), user, userReminder.Name);

            return true;
        }

        #region Conditions
        protected async Task<bool> CheckConditions(UserReminder userReminder, User user)
        {
            if (userReminder.Conditions.Count == 0)
                return true;


            bool cond = false;
            foreach (var item in userReminder.Conditions)
            {
                if (item.ConditionType == UserReminderConditionTypeEnum.UserTag)
                {
                    cond = ConditionUserTag(item, user);
                }
                if (item.ConditionType == UserReminderConditionTypeEnum.UserRegisterField)
                {
                    cond = ConditionUserRegister(item, user);
                }
                if (item.ConditionType == UserReminderConditionTypeEnum.CustomUserAttribute)
                {
                    cond = await ConditionUserAttribute(item, user);
                }
            }

            return cond;
        }
        protected async Task<bool> ConditionNotebook(UserReminder.ReminderCondition condition, ICollection<string> notes)
        {
            bool cond = false;
            if (condition.Condition == UserReminderConditionEnum.AllOfThem)
            {
                cond = true;
                foreach (var item in condition.Notebooks)
                {
                    foreach (var note in notes)
                    {
                        var pr = await _noteService.GetNoteById(note);
                        if (pr != null)
                        {
                            if (pr.NoteNotebooks.Where(x => x.NotebookId == item).Count() == 0)
                                return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            if (condition.Condition == UserReminderConditionEnum.OneOfThem)
            {
                foreach (var item in condition.Notebooks)
                {
                    foreach (var note in notes)
                    {
                        var pr = await _noteService.GetNoteById(note);
                        if (pr != null)
                        {
                            if (pr.NoteNotebooks.Where(x => x.NotebookId == item).Count() > 0)
                                return true;
                        }
                    }
                }
            }

            return cond;
        }
        protected bool ConditionNotes(UserReminder.ReminderCondition condition, ICollection<string> notes)
        {
            bool cond = true;
            if (condition.Condition == UserReminderConditionEnum.AllOfThem)
            {
                cond = notes.ContainsAll(condition.Notes);
            }
            if (condition.Condition == UserReminderConditionEnum.OneOfThem)
            {
                cond = notes.ContainsAny(condition.Notes);
            }

            return cond;
        }

        protected bool ConditionUserTag(UserReminder.ReminderCondition condition, User user)
        {
            bool cond = false;
            if (user != null)
            {
                var userTags = user.UserTags;
                if (condition.Condition == UserReminderConditionEnum.AllOfThem)
                {
                    cond = userTags.Select(x => x).ContainsAll(condition.UserTags);
                }
                if (condition.Condition == UserReminderConditionEnum.OneOfThem)
                {
                    cond = userTags.Select(x => x).ContainsAny(condition.UserTags);
                }
            }
            return cond;
        }

        protected bool ConditionUserRegister(UserReminder.ReminderCondition condition, User user)
        {
            bool cond = false;
            if (user != null)
            {
                if (condition.Condition == UserReminderConditionEnum.AllOfThem)
                {
                    cond = true;
                    foreach (var item in condition.UserRegistration)
                    {
                        if (user.GenericAttributes.Where(x => x.Key == item.RegisterField && x.Value == item.RegisterValue).Count() == 0)
                            cond = false;
                    }
                }
                if (condition.Condition == UserReminderConditionEnum.OneOfThem)
                {
                    foreach (var item in condition.UserRegistration)
                    {
                        if (user.GenericAttributes.Where(x => x.Key == item.RegisterField && x.Value == item.RegisterValue).Count() > 0)
                            cond = true;
                    }
                }
            }
            return cond;
        }

        protected async Task<bool> ConditionUserAttribute(UserReminder.ReminderCondition condition, User user)
        {
            bool cond = false;
            if (user != null)
            {
                if (condition.Condition == UserReminderConditionEnum.AllOfThem)
                {
                    var customUserAttributes = user.GenericAttributes.FirstOrDefault(x => x.Key == "CustomUserAttributes");
                    if (customUserAttributes != null)
                    {
                        if (!String.IsNullOrEmpty(customUserAttributes.Value))
                        {
                            var selectedValues = await _userAttributeParser.ParseUserAttributeValues(customUserAttributes.Value);
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
                if (condition.Condition == UserReminderConditionEnum.OneOfThem)
                {

                    var customUserAttributes = user.GenericAttributes.FirstOrDefault(x => x.Key == "CustomUserAttributes");
                    if (customUserAttributes != null)
                    {
                        if (!String.IsNullOrEmpty(customUserAttributes.Value))
                        {
                            var selectedValues = await _userAttributeParser.ParseUserAttributeValues(customUserAttributes.Value);
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
        #endregion

        #region History

        protected async Task UpdateHistory(User user, UserReminder userReminder, string reminderlevelId, UserReminderHistory history)
        {
            if (history != null)
            {
                history.Levels.Add(new UserReminderHistory.HistoryLevel() {
                    Level = userReminder.Levels.FirstOrDefault(x => x.Id == reminderlevelId).Level,
                    ReminderLevelId = reminderlevelId,
                    SendDate = DateTime.UtcNow,
                });
                if (userReminder.Levels.Max(x => x.Level) ==
                    userReminder.Levels.FirstOrDefault(x => x.Id == reminderlevelId).Level)
                {
                    history.Status = (int)UserReminderHistoryStatusEnum.CompletedReminder;
                    history.EndDate = DateTime.UtcNow;
                }
                await _userReminderHistoryRepository.UpdateAsync(history);
            }
            else
            {
                history = new UserReminderHistory();
                history.UserId = user.Id;
                history.Status = (int)UserReminderHistoryStatusEnum.Started;
                history.StartDate = DateTime.UtcNow;
                history.UserReminderId = userReminder.Id;
                history.ReminderRuleId = userReminder.ReminderRuleId;
                history.Levels.Add(new UserReminderHistory.HistoryLevel() {
                    Level = userReminder.Levels.FirstOrDefault(x => x.Id == reminderlevelId).Level,
                    ReminderLevelId = reminderlevelId,
                    SendDate = DateTime.UtcNow,
                });

                await _userReminderHistoryRepository.InsertAsync(history);
            }

        }

        ////protected async Task UpdateHistory(UserReminder userReminder, string reminderlevelId, UserReminderHistory history)
        ////{
        ////    if (history != null)
        ////    {
        ////        history.Levels.Add(new UserReminderHistory.HistoryLevel() {
        ////            Level = userReminder.Levels.FirstOrDefault(x => x.Id == reminderlevelId).Level,
        ////            ReminderLevelId = reminderlevelId,
        ////            SendDate = DateTime.UtcNow,
        ////        });
        ////        if (userReminder.Levels.Max(x => x.Level) ==
        ////            userReminder.Levels.FirstOrDefault(x => x.Id == reminderlevelId).Level)
        ////        {
        ////            history.Status = (int)UserReminderHistoryStatusEnum.CompletedReminder;
        ////            history.EndDate = DateTime.UtcNow;
        ////        }
        ////        await _userReminderHistoryRepository.UpdateAsync(history);
        ////    }
        ////    else
        ////    {
        ////        history = new UserReminderHistory();
        ////        history.UserId = order.UserId;
        ////        history.Status = (int)UserReminderHistoryStatusEnum.Started;
        ////        history.StartDate = DateTime.UtcNow;
        ////        history.UserReminderId = userReminder.Id;
        ////        history.ReminderRuleId = userReminder.ReminderRuleId;
        ////        history.Levels.Add(new UserReminderHistory.HistoryLevel() {
        ////            Level = userReminder.Levels.FirstOrDefault(x => x.Id == reminderlevelId).Level,
        ////            ReminderLevelId = reminderlevelId,
        ////            SendDate = DateTime.UtcNow,
        ////        });

        ////        await _userReminderHistoryRepository.InsertAsync(history);
        ////    }

        ////}
        ///
        protected async Task CloseHistoryReminder(UserReminder userReminder, UserReminderHistory history)
        {
            history.Status = (int)UserReminderHistoryStatusEnum.CompletedReminder;
            history.EndDate = DateTime.UtcNow;
            await _userReminderHistoryRepository.UpdateAsync(history);
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Gets user reminder
        /// </summary>
        /// <param name="id">User reminder identifier</param>
        /// <returns>User reminder</returns>
        public virtual Task<UserReminder> GetUserReminderById(string id)
        {
            return _userReminderRepository.GetByIdAsync(id);
        }


        /// <summary>
        /// Gets all user reminders
        /// </summary>
        /// <returns>User reminders</returns>
        public virtual async Task<IList<UserReminder>> GetUserReminders()
        {
            var query = from p in _userReminderRepository.Table
                        orderby p.DisplayOrder
                        select p;
            return await query.ToListAsync();
        }

        /// <summary>
        /// Inserts a user reminder
        /// </summary>
        /// <param name="UserReminder">User reminder</param>
        public virtual async Task InsertUserReminder(UserReminder userReminder)
        {
            if (userReminder == null)
                throw new ArgumentNullException("userReminder");

            await _userReminderRepository.InsertAsync(userReminder);

            //event notification
            await _mediator.EntityInserted(userReminder);

        }

        /// <summary>
        /// Delete a user reminder
        /// </summary>
        /// <param name="userReminder">User reminder</param>
        public virtual async Task DeleteUserReminder(UserReminder userReminder)
        {
            if (userReminder == null)
                throw new ArgumentNullException("userReminder");

            await _userReminderRepository.DeleteAsync(userReminder);

            //event notification
            await _mediator.EntityDeleted(userReminder);

        }

        /// <summary>
        /// Updates the user reminder
        /// </summary>
        /// <param name="UserReminder">User reminder</param>
        public virtual async Task UpdateUserReminder(UserReminder userReminder)
        {
            if (userReminder == null)
                throw new ArgumentNullException("userReminder");

            await _userReminderRepository.UpdateAsync(userReminder);

            //event notification
            await _mediator.EntityUpdated(userReminder);
        }



        public virtual async Task<IPagedList<SerializeUserReminderHistory>> GetAllUserReminderHistory(string userReminderId, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = from h in _userReminderHistoryRepository.Table
                        from l in h.Levels
                        select new SerializeUserReminderHistory() { UserId = h.UserId, Id = h.Id, UserReminderId = h.UserReminderId, Level = l.Level, SendDate = l.SendDate };

            query = from p in query
                    where p.UserReminderId == userReminderId
                    select p;
            return await PagedList<SerializeUserReminderHistory>.Create(query, pageIndex, pageSize);
        }

        #endregion

        #region Tasks

        public virtual async Task Task_RegisteredUser(string id = "")
        {
            var datetimeUtcNow = DateTime.UtcNow.Date;
            var userReminder = new List<UserReminder>();
            if (String.IsNullOrEmpty(id))
            {
                userReminder = await (from cr in _userReminderRepository.Table
                                          where cr.Active && datetimeUtcNow >= cr.StartDateTimeUtc && datetimeUtcNow <= cr.EndDateTimeUtc
                                          && cr.ReminderRuleId == (int)UserReminderRuleEnum.RegisteredUser
                                          select cr).ToListAsync();
            }
            else
            {
                userReminder = await (from cr in _userReminderRepository.Table
                                          where cr.Id == id && cr.ReminderRuleId == (int)UserReminderRuleEnum.RegisteredUser
                                          select cr).ToListAsync();
            }
            foreach (var reminder in userReminder)
            {
                var users = await (from cu in _userRepository.Table
                                       where cu.CreatedOnUtc > reminder.LastUpdateDate && cu.Active && !cu.Deleted
                                       && (!String.IsNullOrEmpty(cu.Email))
                                       && !cu.IsSystemAccount
                                       select cu).ToListAsync();

                foreach (var user in users)
                {
                    var history = await (from hc in _userReminderHistoryRepository.Table
                                         where hc.UserId == user.Id && hc.UserReminderId == reminder.Id
                                         select hc).ToListAsync();
                    if (history.Any())
                    {
                        var activereminderhistory = history.FirstOrDefault(x => x.HistoryStatus == UserReminderHistoryStatusEnum.Started);
                        if (activereminderhistory != null)
                        {
                            var lastLevel = activereminderhistory.Levels.OrderBy(x => x.SendDate).LastOrDefault();
                            var reminderLevel = reminder.Levels.FirstOrDefault(x => x.Level > lastLevel.Level);
                            if (reminderLevel != null)
                            {
                                if (DateTime.UtcNow > lastLevel.SendDate.AddDays(reminderLevel.Day).AddHours(reminderLevel.Hour).AddMinutes(reminderLevel.Minutes))
                                {
                                    var send = await SendEmail(user, reminder, reminderLevel.Id);
                                    if (send)
                                        await UpdateHistory(user, reminder, reminderLevel.Id, activereminderhistory);
                                }
                            }
                            else
                            {
                                await CloseHistoryReminder(reminder, activereminderhistory);
                            }
                        }
                        else
                        {
                            if (DateTime.UtcNow > history.Max(x => x.EndDate).AddDays(reminder.RenewedDay) && reminder.AllowRenew)
                            {
                                var level = reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() != null ? reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() : null;
                                if (level != null)
                                {

                                    if (DateTime.UtcNow > user.CreatedOnUtc.AddDays(level.Day).AddHours(level.Hour).AddMinutes(level.Minutes))
                                    {
                                        if (await CheckConditions(reminder, user))
                                        {
                                            var send = await SendEmail(user, reminder, level.Id);
                                            if (send)
                                                await UpdateHistory(user, reminder, level.Id, null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var level = reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() != null ? reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() : null;
                        if (level != null)
                        {

                            if (DateTime.UtcNow > user.CreatedOnUtc.AddDays(level.Day).AddHours(level.Hour).AddMinutes(level.Minutes))
                            {
                                if (await CheckConditions(reminder, user))
                                {
                                    var send = await SendEmail(user, reminder, level.Id);
                                    if (send)
                                        await UpdateHistory(user, reminder, level.Id, null);
                                }
                            }
                        }
                    }
                }
            }
        }

        public virtual async Task Task_LastActivity(string id = "")
        {
            var datetimeUtcNow = DateTime.UtcNow.Date;
            var userReminder = new List<UserReminder>();
            if (String.IsNullOrEmpty(id))
            {
                userReminder = await (from cr in _userReminderRepository.Table
                                          where cr.Active && datetimeUtcNow >= cr.StartDateTimeUtc && datetimeUtcNow <= cr.EndDateTimeUtc
                                          && cr.ReminderRuleId == (int)UserReminderRuleEnum.LastActivity
                                          select cr).ToListAsync();
            }
            else
            {
                userReminder = await (from cr in _userReminderRepository.Table
                                          where cr.Id == id && cr.ReminderRuleId == (int)UserReminderRuleEnum.LastActivity
                                          select cr).ToListAsync();
            }
            foreach (var reminder in userReminder)
            {
                var users = await (from cu in _userRepository.Table
                                       where cu.LastActivityDateUtc < reminder.LastUpdateDate && cu.Active && !cu.Deleted
                                       && (!String.IsNullOrEmpty(cu.Email))
                                       select cu).ToListAsync();

                foreach (var user in users)
                {
                    var history = await (from hc in _userReminderHistoryRepository.Table
                                         where hc.UserId == user.Id && hc.UserReminderId == reminder.Id
                                         select hc).ToListAsync();
                    if (history.Any())
                    {
                        var activereminderhistory = history.FirstOrDefault(x => x.HistoryStatus == UserReminderHistoryStatusEnum.Started);
                        if (activereminderhistory != null)
                        {
                            var lastLevel = activereminderhistory.Levels.OrderBy(x => x.SendDate).LastOrDefault();
                            var reminderLevel = reminder.Levels.FirstOrDefault(x => x.Level > lastLevel.Level);
                            if (reminderLevel != null)
                            {
                                if (DateTime.UtcNow > lastLevel.SendDate.AddDays(reminderLevel.Day).AddHours(reminderLevel.Hour).AddMinutes(reminderLevel.Minutes))
                                {
                                    var send = await SendEmail(user, reminder, reminderLevel.Id);
                                    if (send)
                                        await UpdateHistory(user, reminder, reminderLevel.Id, activereminderhistory);
                                }
                            }
                            else
                            {
                                await CloseHistoryReminder(reminder, activereminderhistory);
                            }
                        }
                        else
                        {
                            if (DateTime.UtcNow > history.Max(x => x.EndDate).AddDays(reminder.RenewedDay) && reminder.AllowRenew)
                            {
                                var level = reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() != null ? reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() : null;
                                if (level != null)
                                {

                                    if (DateTime.UtcNow > user.LastActivityDateUtc.AddDays(level.Day).AddHours(level.Hour).AddMinutes(level.Minutes))
                                    {
                                        if (await CheckConditions(reminder, user))
                                        {
                                            var send = await SendEmail(user, reminder, level.Id);
                                            if (send)
                                                await UpdateHistory(user, reminder, level.Id, null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var level = reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() != null ? reminder.Levels.OrderBy(x => x.Level).FirstOrDefault() : null;
                        if (level != null)
                        {
                            if (DateTime.UtcNow > user.LastActivityDateUtc.AddDays(level.Day).AddHours(level.Hour).AddMinutes(level.Minutes))
                            {
                                if (await CheckConditions(reminder, user))
                                {
                                    var send = await SendEmail(user, reminder, level.Id);
                                    if (send)
                                        await UpdateHistory(user, reminder, level.Id, null);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }

    public class SerializeUserReminderHistory
    {
        public string Id { get; set; }
        public string UserReminderId { get; set; }
        public string UserId { get; set; }
        public DateTime SendDate { get; set; }
        public int Level { get; set; }
    }
}