using ForeverNote.Core;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Services.Commands.Models.Common;
using ForeverNote.Services.Common;
using ForeverNote.Services.Customers;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Messages.DotLiquidDrops;
using ForeverNote.Services.Queries.Models.Customers;
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

        #region Customer workflow

        /// <summary>
        /// Sends 'New customer' notification message to a store owner
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="store">Store identifier</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendCustomerRegisteredNotificationMessage(Customer customer, string languageId)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("NewCustomer.Notification");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddCustomerTokens(liquidObject, customer, language);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = emailAccount.Email;
            var toName = emailAccount.DisplayName;
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        /// <summary>
        /// Sends a welcome message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendCustomerWelcomeMessage(Customer customer, string languageId)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("Customer.WelcomeMessage");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddCustomerTokens(liquidObject, customer, language);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = customer.Email;
            var toName = customer.GetFullName();
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        /// <summary>
        /// Sends an email validation message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="store">Store</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendCustomerEmailValidationMessage(Customer customer, string languageId)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("Customer.EmailValidationMessage");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddCustomerTokens(liquidObject, customer, language);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = customer.Email;
            var toName = customer.GetFullName();
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        /// <summary>
        /// Sends password recovery message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendCustomerPasswordRecoveryMessage(Customer customer, string languageId)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("Customer.PasswordRecovery");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddCustomerTokens(liquidObject, customer, language);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = customer.Email;
            var toName = customer.GetFullName();
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        /// <summary>
        /// Sends a new customer note added notification to a customer
        /// </summary>
        /// <param name="customerNote">Customer note</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendNewCustomerNoteAddedCustomerNotification(CustomerNote customerNote, string languageId)
        {
            if (customerNote == null)
                throw new ArgumentNullException("customerNote");

            var messageTemplate = await GetMessageTemplate("Customer.NewCustomerNote");
            if (messageTemplate == null)
                return 0;

            var language = await EnsureLanguageIsActive(languageId);

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

            LiquidObject liquidObject = new LiquidObject();

            var customer = await _mediator.Send(new GetCustomerByIdQuery() { Id = customerNote.CustomerId });
            if (customer != null)
                await _messageTokenProvider.AddCustomerTokens(liquidObject, customer, language);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = customer.Email;
            var toName = string.Format("{0} {1}", customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.FirstName), customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.LastName));
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        /// <summary>
        /// Send an email token validation message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="token">Token</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendCustomerEmailTokenValidationMessage(Customer customer, string token, string languageId)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("Customer.EmailTokenValidationMessage");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddCustomerTokens(liquidObject, customer, language);
            liquidObject.AdditionalTokens.Add("Token", token);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = customer.Email;
            var toName = customer.GetFullName();
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        #endregion


        #region Newsletter workflow

        /// <summary>
        /// Sends a newsletter subscription activation message
        /// </summary>
        /// <param name="subscription">Newsletter subscription</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendNewsLetterSubscriptionActivationMessage(NewsLetterSubscription subscription,
            string languageId)
        {
            if (subscription == null)
                throw new ArgumentNullException("subscription");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("NewsLetterSubscription.ActivationMessage");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = subscription.Email;
            var toName = "";
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        /// <summary>
        /// Sends a newsletter subscription deactivation message
        /// </summary>
        /// <param name="subscription">Newsletter subscription</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendNewsLetterSubscriptionDeactivationMessage(NewsLetterSubscription subscription,
            string languageId)
        {
            if (subscription == null)
                throw new ArgumentNullException("subscription");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("NewsLetterSubscription.DeactivationMessage");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = subscription.Email;
            var toName = "";
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        #endregion

        #region Send a message to a friend, ask question

        /// <summary>
        /// Sends "email a friend" message
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="product">Product instance</param>
        /// <param name="customerEmail">Customer's email</param>
        /// <param name="friendsEmail">Friend's email</param>
        /// <param name="personalMessage">Personal message</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendProductEmailAFriendMessage(Customer customer, string languageId,
            Product product, string customerEmail, string friendsEmail, string personalMessage)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (product == null)
                throw new ArgumentNullException("product");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("Service.EmailAFriend");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddCustomerTokens(liquidObject, customer, language);
            await _messageTokenProvider.AddProductTokens(liquidObject, product, language);
            liquidObject.EmailAFriend = new LiquidEmailAFriend(personalMessage, customerEmail);

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
        /// <param name="customer">Customer</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="customerEmail">Customer's email</param>
        /// <param name="friendsEmail">Friend's email</param>
        /// <param name="personalMessage">Personal message</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendWishlistEmailAFriendMessage(Customer customer, string languageId,
             string customerEmail, string friendsEmail, string personalMessage)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("Wishlist.EmailAFriend");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddCustomerTokens(liquidObject, customer, language);

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
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="product">Product instance</param>
        /// <param name="customerEmail">Customer's email</param>
        /// <param name="fullName">Friend's name</param>
        /// <param name="message">Personal message</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendProductQuestionMessage(Customer customer, string languageId,
            Product product, string customerEmail, string fullName, string phone, string message)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (product == null)
                throw new ArgumentNullException("product");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("Service.AskQuestion");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddCustomerTokens(liquidObject, customer, language);
            await _messageTokenProvider.AddProductTokens(liquidObject, product, language);
            liquidObject.AskQuestion = new LiquidAskQuestion(message, customerEmail, fullName, phone);

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
                    CustomerId = customer.Id,
                    Email = customerEmail,
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
                toEmail, toName, replyToEmailAddress: customerEmail);
        }

        #endregion

        #region Misc

        /// <summary>
        /// Sends a 'Back in stock' notification message to a customer
        /// </summary>
        /// <param name="subscription">Subscription</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendBackInStockNotification(Customer customer, Product product, BackInStockSubscription subscription, string languageId)
        {
            if (subscription == null)
                throw new ArgumentNullException("subscription");

            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await GetMessageTemplate("Customer.BackInStock");
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            if (customer != null)
                await _messageTokenProvider.AddCustomerTokens(liquidObject, customer, language);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = customer.Email;
            var toName = customer.GetFullName();
            return await SendNotification(messageTemplate, emailAccount,
                languageId, liquidObject,
                toEmail, toName);
        }

        /// <summary>
        /// Sends "contact us" message
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="senderEmail">Sender email</param>
        /// <param name="senderName">Sender name</param>
        /// <param name="subject">Email subject. Pass null if you want a message template subject to be used.</param>
        /// <param name="body">Email body</param>
        /// <param name="attrInfo">Attr info</param>
        /// <param name="attrXml">Attr xml</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendContactUsMessage(Customer customer, string languageId, string senderEmail,
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
            await _messageTokenProvider.AddCustomerTokens(liquidObject, customer, language);
            liquidObject.ContactUs = new LiquidContactUs(senderEmail, senderName, body, attrInfo);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = emailAccount.Email;
            var toName = emailAccount.DisplayName;

            //store in database
            if (_commonSettings.StoreInDatabaseContactUsForm)
            {
                await _mediator.Send(new InsertContactUsCommand() {
                    CustomerId = customer.Id,
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

        #region Customer Action Event

        /// <summary>
        /// Sends a customer action event - Add to cart notification to a customer
        /// </summary>
        /// <param name="CustomerAction">Customer action</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendCustomerActionEvent_Notification(CustomerAction action, string languageId, Customer customer)
        {
            var language = await EnsureLanguageIsActive(languageId);

            var messageTemplate = await _messageTemplateService.GetMessageTemplateById(action.MessageTemplateId);
            if (messageTemplate == null)
                return 0;

            //email account
            var emailAccount = await GetEmailAccountOfMessageTemplate(messageTemplate, language.Id);

            LiquidObject liquidObject = new LiquidObject();
            await _messageTokenProvider.AddCustomerTokens(liquidObject, customer, language);

            //event notification
            await _mediator.MessageTokensAdded(messageTemplate, liquidObject);

            var toEmail = customer.Email;
            var toName = customer.GetFullName();

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
