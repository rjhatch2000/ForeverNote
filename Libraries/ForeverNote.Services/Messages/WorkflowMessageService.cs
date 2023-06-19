using ForeverNote.Core;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Core.Domain.Notes;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Messages.DotLiquidDrops;
using ForeverNote.Services.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ForeverNote.Services.Messages
{
    public partial class WorkflowMessageService : IWorkflowMessageService
    {
        #region Fields

        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly ILanguageService _languageService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IMediator _mediator;

        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly CommonSettings _commonSettings;

        #endregion

        #region Ctor

        public WorkflowMessageService(IMessageTemplateService messageTemplateService,
            IQueuedEmailService queuedEmailService,
            ILanguageService languageService,
            IEmailAccountService emailAccountService,
            IMessageTokenProvider messageTokenProvider,
            IMediator mediator,
            EmailAccountSettings emailAccountSettings,
            CommonSettings commonSettings)
        {
            _messageTemplateService = messageTemplateService;
            _queuedEmailService = queuedEmailService;
            _languageService = languageService;
            _emailAccountService = emailAccountService;
            _messageTokenProvider = messageTokenProvider;
            _emailAccountSettings = emailAccountSettings;
            _commonSettings = commonSettings;
            _mediator = mediator;
        }

        #endregion

        #region Utilities

        protected virtual async Task<MessageTemplate> GetMessageTemplate(string messageTemplateName)
        {
            var messageTemplate = await _messageTemplateService.GetMessageTemplateByName(messageTemplateName);

            //no template found
            if (messageTemplate == null)
                return null;

            //ensure it's active
            var isActive = messageTemplate.IsActive;
            if (!isActive)
                return null;

            return messageTemplate;
        }

        protected virtual async Task<EmailAccount> GetEmailAccountOfMessageTemplate(MessageTemplate messageTemplate, string languageId)
        {
            var emailAccounId = messageTemplate.GetLocalized(mt => mt.EmailAccountId, languageId);
            var emailAccount = await _emailAccountService.GetEmailAccountById(emailAccounId);
            if (emailAccount == null)
                emailAccount = await _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
            if (emailAccount == null)
                emailAccount = (await _emailAccountService.GetAllEmailAccounts()).FirstOrDefault();
            return emailAccount;

        }

        protected virtual async Task<Language> EnsureLanguageIsActive(string languageId)
        {
            //load language by specified ID
            var language = await _languageService.GetLanguageById(languageId);

            if (language == null || !language.Published)
            {
                //load any language from the specified store
                language = (await _languageService.GetAllLanguages()).FirstOrDefault();
            }
            if (language == null || !language.Published)
            {
                //load any language
                language = (await _languageService.GetAllLanguages()).FirstOrDefault();
            }

            if (language == null)
                throw new Exception("No active language could be loaded");
            return language;
        }

        #endregion

        #region Methods

        #region User workflow

        /// <summary>
        /// Sends 'New user' notification message to a store owner
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="store">Store identifier</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendUserRegisteredNotificationMessage(User user, string languageId)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("NewUser.Notification");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddUserTokens(liquidObject, user, language);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = emailAccount.Email;
            var toName = emailAccount.DisplayName;
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        /// <summary>
        /// Sends a welcome message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendUserWelcomeMessage(User user, string languageId)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("User.WelcomeMessage");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddUserTokens(liquidObject, user, language);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = user.Email;
            var toName = user.GetFullName();
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        /// <summary>
        /// Sends an email validation message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendUserEmailValidationMessage(User user, string languageId)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("User.EmailValidationMessage");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddUserTokens(liquidObject, user, language);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = user.Email;
            var toName = user.GetFullName();
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        /// <summary>
        /// Sends password recovery message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendUserPasswordRecoveryMessage(User user, string languageId)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("User.PasswordRecovery");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddUserTokens(liquidObject, user, language);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = user.Email;
            var toName = user.GetFullName();
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        /// <summary>
        /// Send an email token validation message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="token">Token</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendUserEmailTokenValidationMessage(User user, string token, string languageId)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("User.EmailTokenValidationMessage");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddUserTokens(liquidObject, user, language);
            liquidObject.AdditionalTokens.Add("Token", token);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = user.Email;
            var toName = user.GetFullName();
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        #endregion

        #region Send a message to a friend, ask question

        /// <summary>
        /// Sends "email a friend" message
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="note">Note instance</param>
        /// <param name="userEmail">User's email</param>
        /// <param name="friendsEmail">Friend's email</param>
        /// <param name="personalMessage">Personal message</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendNoteEmailAFriendMessage(User user, string languageId,
            Note note, string userEmail, string friendsEmail, string personalMessage)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (note == null)
                throw new ArgumentNullException("note");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("Service.EmailAFriend");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddUserTokens(liquidObject, user, language);
            await _messageTokenProvider.AddNoteTokens(liquidObject, note, language);
            liquidObject.EmailAFriend = new LiquidEmailAFriend(personalMessage, userEmail);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = friendsEmail;
            var toName = "";
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        /// <summary>
        /// Sends wishlist "email a friend" message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="userEmail">User's email</param>
        /// <param name="friendsEmail">Friend's email</param>
        /// <param name="personalMessage">Personal message</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendWishlistEmailAFriendMessage(User user, string languageId,
             string userEmail, string friendsEmail, string personalMessage)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("Wishlist.EmailAFriend");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddUserTokens(liquidObject, user, language);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = friendsEmail;
            var toName = "";
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }


        /// <summary>
        /// Sends "email a friend" message
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="note">Note instance</param>
        /// <param name="userEmail">User's email</param>
        /// <param name="fullName">Friend's name</param>
        /// <param name="message">Personal message</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendNoteQuestionMessage(User user, string languageId,
            Note note, string userEmail, string fullName, string phone, string message)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (note == null)
                throw new ArgumentNullException("note");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("Service.AskQuestion");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddUserTokens(liquidObject, user, language);
            await _messageTokenProvider.AddNoteTokens(liquidObject, note, language);
            liquidObject.AskQuestion = new LiquidAskQuestion(message, userEmail, fullName, phone);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            //store in database
            if (_commonSettings.StoreInDatabaseContactUsForm)
            {
                var subject = messageTemplate.GetLocalized(mt => mt.Subject, languageId);
                var body = messageTemplate.GetLocalized(mt => mt.Body, languageId);

                var subjectReplaced = LiquidExtensions.Render(liquidObject, subject);
                var bodyReplaced = LiquidExtensions.Render(liquidObject, body);

                await _mediator.Send(new InsertContactUsCommand() {
                    UserId = user.Id,
                    Email = userEmail,
                    Enquiry = bodyReplaced,
                    FullName = fullName,
                    Subject = subjectReplaced,
                    EmailAccountId = emailAccount.Id
                });
            }

            var toEmail = emailAccount.Email;
            var toName = "";

            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName, replyToEmailAddress: userEmail);
        }

        #endregion

        #region Misc

        /// <summary>
        /// Sends "contact us" message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="senderEmail">Sender email</param>
        /// <param name="senderName">Sender name</param>
        /// <param name="subject">Email subject. Pass null if you want a message template subject to be used.</param>
        /// <param name="body">Email body</param>
        /// <param name="attrInfo">Attr info</param>
        /// <param name="attrXml">Attr xml</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendContactUsMessage(User user, string languageId, string senderEmail,
            string senderName, string subject, string body, string attrInfo, string attrXml)
        {
            var language = await EnsureLanguageIsActive(languageId);
            var messageTemplate = await GetMessageTemplate("Service.ContactUs");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            string fromEmail;
            string fromName;
            senderName = WebUtility.HtmlEncode(senderName);
            senderEmail = WebUtility.HtmlEncode(senderEmail);
            //required for some SMTP servers
            if (_commonSettings.UseSystemEmailForContactUsForm)
            {
                fromEmail = emailAccount.Email;
                fromName = emailAccount.DisplayName;
            }
            else
            {
                fromEmail = senderEmail;
                fromName = senderName;
            }

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddUserTokens(liquidObject, user, language);
            liquidObject.ContactUs = new LiquidContactUs(senderEmail, senderName, body, attrInfo);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = emailAccount.Email;
            var toName = emailAccount.DisplayName;

            //store in database
            if (_commonSettings.StoreInDatabaseContactUsForm)
            {
                await _mediator.Send(new InsertContactUsCommand() {
                    UserId = user.Id,
                    Email = senderEmail,
                    Enquiry = body,
                    FullName = senderName,
                    Subject = string.IsNullOrEmpty(subject) ? "Contact Us (form)" : subject,
                    ContactAttributeDescription = attrInfo,
                    ContactAttributesXml = attrXml,
                    EmailAccountId = emailAccount.Id
                });
            }
            return await SendNotification(messageTemplate, emailAccount, languageId, liquidObject, toEmail, toName,
                fromEmail: fromEmail,
                fromName: fromName,
                subject: subject,
                replyToEmailAddress: senderEmail,
                replyToName: senderName);
        }

        #region User Action Event

        /// <summary>
        /// Sends a user action event - Add to cart notification to a user
        /// </summary>
        /// <param name="UserAction">User action</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendUserActionEvent_Notification(UserAction action, string languageId, User user)
        {
            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await _messageTemplateService.GetMessageTemplateById(action.MessageTemplateId);
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddUserTokens(liquidObject, user, language);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = user.Email;
            var toName = user.GetFullName();

            if (!String.IsNullOrEmpty(toEmail))
                toEmail = emailAccount.Email;

            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        #endregion

        public virtual async Task<int> SendNotification(MessageTemplate messageTemplate,
            EmailAccount emailAccount, string languageId, LiquidObject liquidObject,
            string toEmailAddress, string toName,
            string attachmentFilePath = null, string attachmentFileName = null,
            IEnumerable<string> attachedDownloads = null,
            string replyToEmailAddress = null, string replyToName = null,
            string fromEmail = null, string fromName = null, string subject = null)
        {
            if (String.IsNullOrEmpty(toEmailAddress))
                return 0;

            //retrieve localized message template data
            var bcc = messageTemplate.GetLocalized(mt => mt.BccEmailAddresses, languageId);

            if (String.IsNullOrEmpty(subject))
                subject = messageTemplate.GetLocalized(mt => mt.Subject, languageId);

            var body = messageTemplate.GetLocalized(mt => mt.Body, languageId);

            var subjectReplaced = LiquidExtensions.Render(liquidObject, subject);
            var bodyReplaced = LiquidExtensions.Render(liquidObject, body);

            var attachments = new List<string>();
            if (attachedDownloads != null)
                attachments.AddRange(attachedDownloads);
            if (!string.IsNullOrEmpty(messageTemplate.AttachedDownloadId))
                attachments.Add(messageTemplate.AttachedDownloadId);

            //limit name length
            toName = CommonHelper.EnsureMaximumLength(toName, 300);
            var email = new QueuedEmail {
                Priority = QueuedEmailPriority.High,
                From = !string.IsNullOrEmpty(fromEmail) ? fromEmail : emailAccount.Email,
                FromName = !string.IsNullOrEmpty(fromName) ? fromName : emailAccount.DisplayName,
                To = toEmailAddress,
                ToName = toName,
                ReplyTo = replyToEmailAddress,
                ReplyToName = replyToName,
                CC = string.Empty,
                Bcc = bcc,
                Subject = subjectReplaced,
                Body = bodyReplaced,
                AttachmentFilePath = attachmentFilePath,
                AttachmentFileName = attachmentFileName,
                AttachedDownloads = attachments,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id,
                DontSendBeforeDateUtc = !messageTemplate.DelayBeforeSend.HasValue ? null
                     : (DateTime?)(DateTime.UtcNow + TimeSpan.FromHours(messageTemplate.DelayPeriod.ToHours(messageTemplate.DelayBeforeSend.Value)))
            };

            await _queuedEmailService.InsertQueuedEmail(email);
            return 1;
        }

        /// <summary>
        /// Sends a test email
        /// </summary>
        /// <param name="messageTemplateId">Message template identifier</param>
        /// <param name="sendToEmail">Send to email</param>
        /// <param name="liquidObject">Tokens</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendTestEmail(string messageTemplateId, string sendToEmail,
            LiquidObject liquidObject, string languageId)
        {
            var messageTemplate = await _messageTemplateService.GetMessageTemplateById(messageTemplateId);
            if (messageTemplate == null)
                throw new ArgumentException("Template cannot be loaded");
            var language = await EnsureLanguageIsActive(languageId);

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                sendToEmail, null);
        }

        #endregion

        #endregion
    }
}
