using ForeverNote.Core;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain;
using ForeverNote.Core.Domain.AdminSearch;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Configuration;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Logging;
using ForeverNote.Core.Domain.Media;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Core.Domain.Notes;
using ForeverNote.Core.Domain.Notebooks;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.PushNotifications;
using ForeverNote.Core.Domain.Security;
using ForeverNote.Core.Domain.Tasks;
using ForeverNote.Core.Infrastructure;
using ForeverNote.Services.Common;
using ForeverNote.Services.Configuration;
using ForeverNote.Services.Users;
using ForeverNote.Services.Helpers;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Media;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Installation
{
    public partial class CodeFirstInstallationService : IInstallationService
    {
        #region Fields

        private readonly IRepository<ForeverNoteDatabaseVersion> _databaseVersionRepository;
        private readonly IRepository<CampaignHistory> _campaignHistoryRepository;
        private readonly IRepository<Language> _languageRepository;
        private readonly IRepository<LocaleStringResource> _lsrRepository;
        private readonly IRepository<Log> _logRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserTagNote> _userTagNoteRepository;
        private readonly IRepository<UserHistoryPassword> _userHistoryPasswordRepository;
        private readonly IRepository<UserApi> _userapiRepository;
        private readonly IRepository<Notebook> _notebookRepository;
        private readonly IRepository<Note> _noteRepository;
        private readonly IRepository<EmailAccount> _emailAccountRepository;
        private readonly IRepository<MessageTemplate> _messageTemplateRepository;
        private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
        private readonly IRepository<NoteTag> _noteTagRepository;
        private readonly IRepository<ScheduleTask> _scheduleTaskRepository;
        private readonly IRepository<SearchTerm> _searchtermRepository;
        private readonly IRepository<Setting> _settingRepository;
        private readonly IRepository<ExternalAuthenticationRecord> _externalAuthenticationRepository;
        private readonly IRepository<ReturnRequestAction> _returnRequestActionRepository;
        private readonly IRepository<ContactUs> _contactUsRepository;
        private readonly IRepository<UserAction> _userAction;
        private readonly IRepository<UserActionType> _userActionType;
        private readonly IRepository<UserActionHistory> _userActionHistory;
        private readonly IRepository<PopupArchive> _popupArchive;
        private readonly IRepository<UserReminderHistory> _userReminderHistoryRepository;
        private readonly IRepository<RecentlyViewedNote> _recentlyViewedNoteRepository;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWebHelper _webHelper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Ctor

        public CodeFirstInstallationService(IServiceProvider serviceProvider)
        {
            _databaseVersionRepository = serviceProvider.GetRequiredService<IRepository<ForeverNoteDatabaseVersion>>();
            _campaignHistoryRepository = serviceProvider.GetRequiredService<IRepository<CampaignHistory>>();
            _languageRepository = serviceProvider.GetRequiredService<IRepository<Language>>();
            _lsrRepository = serviceProvider.GetRequiredService<IRepository<LocaleStringResource>>();
            _logRepository = serviceProvider.GetRequiredService<IRepository<Log>>();
            _userRepository = serviceProvider.GetRequiredService<IRepository<User>>();
            _userTagNoteRepository = serviceProvider.GetRequiredService<IRepository<UserTagNote>>();
            _userHistoryPasswordRepository = serviceProvider.GetRequiredService<IRepository<UserHistoryPassword>>();
            _userapiRepository = serviceProvider.GetRequiredService<IRepository<UserApi>>();
            _notebookRepository = serviceProvider.GetRequiredService<IRepository<Notebook>>();
            _noteRepository = serviceProvider.GetRequiredService<IRepository<Note>>();
            _emailAccountRepository = serviceProvider.GetRequiredService<IRepository<EmailAccount>>();
            _messageTemplateRepository = serviceProvider.GetRequiredService<IRepository<MessageTemplate>>();
            _activityLogTypeRepository = serviceProvider.GetRequiredService<IRepository<ActivityLogType>>();
            _noteTagRepository = serviceProvider.GetRequiredService<IRepository<NoteTag>>();
            _recentlyViewedNoteRepository = serviceProvider.GetRequiredService<IRepository<RecentlyViewedNote>>();
            _scheduleTaskRepository = serviceProvider.GetRequiredService<IRepository<ScheduleTask>>();
            _searchtermRepository = serviceProvider.GetRequiredService<IRepository<SearchTerm>>();
            _settingRepository = serviceProvider.GetRequiredService<IRepository<Setting>>();
            _externalAuthenticationRepository = serviceProvider.GetRequiredService<IRepository<ExternalAuthenticationRecord>>();
            _contactUsRepository = serviceProvider.GetRequiredService<IRepository<ContactUs>>();
            _returnRequestActionRepository = serviceProvider.GetRequiredService<IRepository<ReturnRequestAction>>();
            _userAction = serviceProvider.GetRequiredService<IRepository<UserAction>>();
            _userActionType = serviceProvider.GetRequiredService<IRepository<UserActionType>>();
            _userActionHistory = serviceProvider.GetRequiredService<IRepository<UserActionHistory>>();
            _userReminderHistoryRepository = serviceProvider.GetRequiredService<IRepository<UserReminderHistory>>();
            _popupArchive = serviceProvider.GetRequiredService<IRepository<PopupArchive>>();
            _genericAttributeService = serviceProvider.GetRequiredService<IGenericAttributeService>();
            _webHelper = serviceProvider.GetRequiredService<IWebHelper>();
            _hostingEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Utilities

        protected virtual string GetSamplesPath()
        {
            return Path.Combine(_hostingEnvironment.WebRootPath, "content/samples/");
        }


        protected virtual async Task InstallVersion()
        {
            var version = new ForeverNoteDatabaseVersion
            {
                Version = Core.ForeverNoteVersion.CurrentVersion
            };
            await _databaseVersionRepository.InsertAsync(version);
        }

        protected virtual async Task InstallLanguages()
        {
            var language = new Language {
                Name = "English",
                LanguageCulture = "en-US",
                UniqueSeoCode = "en",
                FlagImageFileName = "us.png",
                Published = true,
                DisplayOrder = 1
            };
            await _languageRepository.InsertAsync(language);
        }

        protected virtual async Task InstallLocaleResources()
        {
            //'English' language
            var language = _languageRepository.Table.Single(l => l.Name == "English");

            //save resources
            foreach (var filePath in System.IO.Directory.EnumerateFiles(CommonHelper.MapPath("~/App_Data/Localization/"), "*.forevernoteres.xml", SearchOption.TopDirectoryOnly))
            {
                var localesXml = File.ReadAllText(filePath);
                var localizationService = _serviceProvider.GetRequiredService<ILocalizationService>();
                await localizationService.ImportResourcesFromXmlInstall(language, localesXml);
            }

        }

        protected virtual async Task InstallUsersAndUsers(string defaultUserEmail, string defaultUserPassword)
        {
            //admin user
            var adminUser = new User {
                UserGuid = Guid.NewGuid(),
                Email = defaultUserEmail,
                Username = defaultUserEmail,
                Password = defaultUserPassword,
                PasswordFormat = PasswordFormat.Clear,
                PasswordSalt = "",
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
                PasswordChangeDateUtc = DateTime.UtcNow,
            };
            await _userRepository.InsertAsync(adminUser);

            //set default user name
            await _genericAttributeService.SaveAttribute(adminUser, SystemUserAttributeNames.FirstName, "John");
            await _genericAttributeService.SaveAttribute(adminUser, SystemUserAttributeNames.LastName, "Smith");

            //built-in user for background tasks
            var backgroundTaskUser = new User {
                Email = "builtin@background-task-record.com",
                UserGuid = Guid.NewGuid(),
                PasswordFormat = PasswordFormat.Clear,
                Active = true,
                IsSystemAccount = true,
                SystemName = SystemUserNames.BackgroundTask,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
            };
            await _userRepository.InsertAsync(backgroundTaskUser);
        }

        protected virtual async Task HashDefaultUserPassword(string defaultUserEmail, string defaultUserPassword)
        {
            var userRegistrationService = _serviceProvider.GetRequiredService<IUserRegistrationService>();
            await userRegistrationService.ChangePassword(new ChangePasswordRequest(defaultUserEmail, false, PasswordFormat.Hashed, defaultUserPassword));
        }

        protected virtual async Task InstallUserAction()
        {
            var userActionType = new List<UserActionType>()
            {
                new UserActionType()
                {
                    Name = "Add to cart",
                    SystemKeyword = "AddToCart",
                    Enabled = false,
                    ConditionType = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 13 }
                },
                new UserActionType()
                {
                    Name = "Add order",
                    SystemKeyword = "AddOrder",
                    Enabled = false,
                    ConditionType = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 13 }
                },
                new UserActionType()
                {
                    Name = "Paid order",
                    SystemKeyword = "PaidOrder",
                    Enabled = false,
                    ConditionType = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 13 }
                },
                new UserActionType()
                {
                    Name = "Viewed",
                    SystemKeyword = "Viewed",
                    Enabled = false,
                    ConditionType = {1, 2, 3, 7, 8, 9, 10, 13}
                },
                new UserActionType()
                {
                    Name = "Url",
                    SystemKeyword = "Url",
                    Enabled = false,
                    ConditionType = {7, 8, 9, 10, 11, 12, 13}
                },
                new UserActionType()
                {
                    Name = "User Registration",
                    SystemKeyword = "Registration",
                    Enabled = false,
                    ConditionType = {7, 8, 9, 10, 13}
                }
            };
            await _userActionType.InsertAsync(userActionType);

        }

        protected virtual async Task InstallEmailAccounts()
        {
            var emailAccounts = new List<EmailAccount>
                               {
                                   new EmailAccount
                                       {
                                           Email = "test@mail.com",
                                           DisplayName = "Store name",
                                           Host = "smtp.mail.com",
                                           Port = 25,
                                           Username = "123",
                                           Password = "123",
                                           SecureSocketOptionsId = 1,
                                           UseServerCertificateValidation = true
                                       },
                               };
            await _emailAccountRepository.InsertAsync(emailAccounts);
        }

        protected virtual async Task InstallMessageTemplates()
        {
            var eaGeneral = _emailAccountRepository.Table.FirstOrDefault();
            if (eaGeneral == null)
                throw new Exception("Default email account cannot be loaded");

            var OrderNotes = File.ReadAllText(CommonHelper.MapPath("~/App_Data/Upgrade/Order.Notes.txt"));
            var OrderVendorNotes = File.ReadAllText(CommonHelper.MapPath("~/App_Data/Upgrade/Order.VendorNotes.txt"));
            var ShipmentNotes = File.ReadAllText(CommonHelper.MapPath("~/App_Data/Upgrade/Shipment.Notes.txt"));

            var messageTemplates = new List<MessageTemplate>
                               {
                                    new MessageTemplate
                                       {
                                           Name = "AuctionEnded.UserNotificationWin",
                                           Subject = "{{Store.Name}}. Auction ended.",
                                           Body = "<p>Hello, {{User.FullName}}!</p><p></p><p>At {{Auctions.EndTime}} you have won <a href=\"{{Store.URL}}{{Auctions.NoteSeName}}\">{{Auctions.NoteName}}</a> for {{Auctions.Price}}. Visit  <a href=\"{{Store.URL}}/cart\">cart</a> to finish checkout process. </p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                    new MessageTemplate
                                            {
                                                Name = "AuctionEnded.UserNotificationLost",
                                                Subject = "{{Store.Name}}. Auction ended.",
                                                Body = "<p>Hello, {{User.FullName}}!</p><p></p><p>Unfortunately you did not win the bid {{Auctions.NoteName}}</p> <p>End price:  {{Auctions.Price}} </p> <p>End date auction {{Auctions.EndTime}} </p>",
                                                IsActive = true,
                                                EmailAccountId = eaGeneral.Id,
                                            },
                                    new MessageTemplate
                                            {
                                                Name = "AuctionEnded.UserNotificationBin",
                                                Subject = "{{Store.Name}}. Auction ended.",
                                                Body = "<p>Hello, {{User.FullName}}!</p><p></p><p>Unfortunately you did not win the bid {{Note.Name}}</p> <p>Note was bought by option Buy it now for price: {{Note.Price}} </p>",
                                                IsActive = true,
                                                EmailAccountId = eaGeneral.Id,
                                            },
                                    new MessageTemplate
                                       {
                                           Name = "AuctionEnded.StoreOwnerNotification",
                                           Subject = "{{Store.Name}}. Auction ended.",
                                           Body = "<p>At {{Auctions.EndTime}} {{User.FullName}} have won <a href=\"{{Store.URL}}{{Auctions.NoteSeName}}\">{{Auctions.NoteName}}</a> for {{Auctions.Price}}.</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                    new MessageTemplate
                                       {
                                           Name = "AuctionExpired.StoreOwnerNotification",
                                           Subject = "Your auction to note {{Note.Name}}  has expired.",
                                           Body = "Hello, <br> Your auction to note {{Note.Name}} has expired without bid.",
                                           IsActive = false,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                    new MessageTemplate
                                       {
                                           Name = "BidUp.UserNotification",
                                           Subject = "{{Store.Name}}. Your offer has been outbid.",
                                           Body = "<p>Hi {{User.FullName}}!</p><p>Your offer for note <a href=\"{{Store.URL}}{{Auctions.NoteSeName}}\">{{Auctions.NoteName}}</a> has been outbid. Your price was {{Auctions.Price}}.<br />\r\nRaise a price by raising one's offer. Auction will be ended on {{Auctions.EndTime}}</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                    new MessageTemplate
                                       {
                                           Name = "Blog.BlogComment",
                                           Subject = "{{Store.Name}}. New blog comment.",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nA new blog comment has been created for blog post \"{{BlogComment.BlogPostTitle}}\".</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "Knowledgebase.ArticleComment",
                                           Subject = "{{Store.Name}}. New article comment.",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nA new article comment has been created for article \"{{Knowledgebase.ArticleCommentTitle}}\".</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "User.BackInStock",
                                           Subject = "{{Store.Name}}. Back in stock notification",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nHello {{User.FullName}}, <br />\r\nNote <a target=\"_blank\" href=\"{{BackInStockSubscription.NoteUrl}}\">{{BackInStockSubscription.NoteName}}</a> is in stock.</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "UserDelete.StoreOwnerNotification",
                                           Subject = "{{Store.Name}}. User has been deleted.",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> ,<br />\r\n{{User.FullName}} ({{User.Email}}) has just deleted from your database. </p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "User.EmailTokenValidationMessage",
                                           Subject = "{{Store.Name}} - Email Verification Code",
                                           Body = "Hello {{User.FullName}}, <br /><br />\r\n Enter this 6 digit code on the sign in page to confirm your identity:<br /><br /> \r\n <b>{{AdditionalTokens[\"Token\"]}}</b><br /><br />\r\n Yours securely, <br /> \r\n Team",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "User.EmailValidationMessage",
                                           Subject = "{{Store.Name}}. Email validation",
                                           Body = "<a href=\"{{Store.URL}}\">{{Store.Name}}</a>  <br />\r\n  <br />\r\n  To activate your account <a href=\"{{User.AccountActivationURL}}\">click here</a>.     <br />\r\n  <br />\r\n  {{Store.Name}}",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "User.NewPM",
                                           Subject = "{{Store.Name}}. You have received a new private message",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nYou have received a new private message.</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "User.PasswordRecovery",
                                           Subject = "{{Store.Name}}. Password recovery",
                                           Body = "<a href=\"{{Store.URL}}\">{{Store.Name}}</a>  <br />\r\n  <br />\r\n  To change your password <a href=\"{{User.PasswordRecoveryURL}}\">click here</a>.     <br />\r\n  <br />\r\n  {{Store.Name}}",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "User.WelcomeMessage",
                                           Subject = "Welcome to {{Store.Name}}",
                                           Body = "We welcome you to <a href=\"{{Store.URL}}\"> {{Store.Name}}</a>.<br />\r\n<br />\r\nYou can now take part in the various services we have to offer you. Some of these services include:<br />\r\n<br />\r\nPermanent Cart - Any notes added to your online cart remain there until you remove them, or check them out.<br />\r\nAddress Book - We can now deliver your notes to another address other than yours! This is perfect to send birthday gifts direct to the birthday-person themselves.<br />\r\nOrder History - View your history of purchases that you have made with us.<br />\r\nNotes Reviews - Share your opinions on notes with our other users.<br />\r\n<br />\r\nFor help with any of our online services, please email the store-owner: <a href=\"mailto:{{Store.Email}}\">{{Store.Email}}</a>.<br />\r\n<br />\r\nNote: This email address was provided on our registration page. If you own the email and did not register on our site, please send an email to <a href=\"mailto:{{Store.Email}}\">{{Store.Email}}</a>.",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "Forums.NewForumPost",
                                           Subject = "{{Store.Name}}. New Post Notification.",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nA new post has been created in the topic <a href=\"{{Forums.TopicURL}}\">\"{{Forums.TopicName}}\"</a> at <a href=\"{{Forums.ForumURL}}\">\"{{Forums.ForumName}}\"</a> forum.<br />\r\n<br />\r\nClick <a href=\"{{Forums.TopicURL}}\">here</a> for more info.<br />\r\n<br />\r\nPost author: {{Forums.PostAuthor}}<br />\r\nPost body: {{Forums.PostBody}}</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "Forums.NewForumTopic",
                                           Subject = "{{Store.Name}}. New Topic Notification.",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nA new topic <a href=\"{{Forums.TopicURL}}\">\"{{Forums.TopicName}}\"</a> has been created at <a href=\"{{Forums.ForumURL}}\">\"{{Forums.ForumName}}\"</a> forum.<br />\r\n<br />\r\nClick <a href=\"{{Forums.TopicURL}}\">here</a> for more info.</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "GiftCard.Notification",
                                           Subject = "{{GiftCard.SenderName}} has sent you a gift card for {{Store.Name}}",
                                           Body = "<p>You have received a gift card for {{Store.Name}}</p><p>Dear {{GiftCard.RecipientName}}, <br />\r\n<br />\r\n{{GiftCard.SenderName}} ({{GiftCard.SenderEmail}}) has sent you a {{GiftCard.Amount}} gift cart for <a href=\"{{Store.URL}}\"> {{Store.Name}}</a></p><p>You gift card code is {{GiftCard.CouponCode}}</p><p>{{GiftCard.Message}}</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "NewUser.Notification",
                                           Subject = "{{Store.Name}}. New user registration",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nA new user registered with your store. Below are the user's details:<br />\r\nFull name: {{User.FullName}}<br />\r\nEmail: {{User.Email}}</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "NewReturnRequest.UserNotification",
                                           Subject = "{{Store.Name}}. New return request.",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nHello {{User.FullName}}!<br />\r\n You have just submitted a new return request. Details are below:<br />\r\nRequest ID: {{ReturnRequest.Id}}<br />\r\nNotes:<br />\r\n{{ReturnRequest.Notes}}<br />\r\nUser comments: {{ReturnRequest.UserComment}}<br />\r\n<br />\r\nPickup date: {{ReturnRequest.PickupDate}}<br />\r\n<br />\r\nPickup address:<br />\r\n{{ReturnRequest.PickupAddressFirstName}} {{ReturnRequest.PickupAddressLastName}}<br />\r\n{{ReturnRequest.PickupAddressAddress1}}<br />\r\n{{ReturnRequest.PickupAddressCity}} {{ReturnRequest.PickupAddressZipPostalCode}}<br />\r\n{{ReturnRequest.PickupAddressStateProvince}} {{ReturnRequest.PickupAddressCountry}}<br />\r\n</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "NewReturnRequest.StoreOwnerNotification",
                                           Subject = "{{Store.Name}}. New return request.",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\n{{User.FullName}} has just submitted a new return request. Details are below:<br />\r\nRequest ID: {{ReturnRequest.Id}}<br />\r\nNotes:<br />\r\n{{ReturnRequest.Notes}}<br />\r\nUser comments: {{ReturnRequest.UserComment}}<br />\r\n<br />\r\nPickup date: {{ReturnRequest.PickupDate}}<br />\r\n<br />\r\nPickup address:<br />\r\n{{ReturnRequest.PickupAddressFirstName}} {{ReturnRequest.PickupAddressLastName}}<br />\r\n{{ReturnRequest.PickupAddressAddress1}}<br />\r\n{{ReturnRequest.PickupAddressCity}} {{ReturnRequest.PickupAddressZipPostalCode}}<br />\r\n{{ReturnRequest.PickupAddressStateProvince}} {{ReturnRequest.PickupAddressCountry}}<br />\r\n</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "News.NewsComment",
                                           Subject = "{{Store.Name}}. New news comment.",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nA new news comment has been created for news \"{{NewsComment.NewsTitle}}\".</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "NewsLetterSubscription.ActivationMessage",
                                           Subject = "{{Store.Name}}. Subscription activation message.",
                                           Body = "<p><a href=\"{{NewsLetterSubscription.ActivationUrl}}\">Click here to confirm your subscription to our list.</a></p><p>If you received this email by mistake, simply delete it.</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "NewsLetterSubscription.DeactivationMessage",
                                           Subject = "{{Store.Name}}. Subscription deactivation message.",
                                           Body = "<p><a href=\"{{NewsLetterSubscription.DeactivationUrl}}\">Click here to unsubscribe from our newsletter.</a></p><p>If you received this email by mistake, simply delete it.</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "NewVATSubmitted.StoreOwnerNotification",
                                           Subject = "{{Store.Name}}. New VAT number is submitted.",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\n{{User.FullName}} ({{User.Email}}) has just submitted a new VAT number. Details are below:<br />\r\nVAT number: {{User.VatNumber}}<br />\r\nVAT number status: {{User.VatNumberStatus}}<br />\r\nReceived name: {{VatValidationResult.Name}}<br />\r\nReceived address: {{VatValidationResult.Address}}</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                  new MessageTemplate
                                       {
                                           Name = "OrderCancelled.StoreOwnerNotification",
                                           Subject = "{{Store.Name}}. User cancelled an order",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\n<br />\r\nUser cancelled an order. Below is the summary of the order. <br />\r\n<br />\r\nOrder Number: {{Order.OrderNumber}}<br />\r\nOrder Details: <a target=\"_blank\" href=\"{{Order.OrderURLForUser}}\">{{Order.OrderURLForUser}}</a><br />\r\nDate Ordered: {{Order.CreatedOn}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nBilling Address<br />\r\n{{Order.BillingFirstName}} {{Order.BillingLastName}}<br />\r\n{{Order.BillingAddress1}}<br />\r\n{{Order.BillingCity}} {{Order.BillingZipPostalCode}}<br />\r\n{{Order.BillingStateProvince}} {{Order.BillingCountry}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nShipping Address<br />\r\n{{Order.ShippingFirstName}} {{Order.ShippingLastName}}<br />\r\n{{Order.ShippingAddress1}}<br />\r\n{{Order.ShippingCity}} {{Order.ShippingZipPostalCode}}<br />\r\n{{Order.ShippingStateProvince}} {{Order.ShippingCountry}}<br />\r\n<br />\r\nShipping Method: {{Order.ShippingMethod}}<br />\r\n<br />\r\n" + OrderNotes + "</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                    new MessageTemplate
                                       {
                                           Name = "OrderCancelled.UserNotification",
                                           Subject = "{{Store.Name}}. Your order cancelled",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nHello {{Order.UserFullName}}, <br />\r\nYour order has been cancelled. Below is the summary of the order. <br />\r\n<br />\r\nOrder Number: {{Order.OrderNumber}}<br />\r\nOrder Details: <a target=\"_blank\" href=\"{{Order.OrderURLForUser}}\">{{Order.OrderURLForUser}}</a><br />\r\nDate Ordered: {{Order.CreatedOn}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nBilling Address<br />\r\n{{Order.BillingFirstName}} {{Order.BillingLastName}}<br />\r\n{{Order.BillingAddress1}}<br />\r\n{{Order.BillingCity}} {{Order.BillingZipPostalCode}}<br />\r\n{{Order.BillingStateProvince}} {{Order.BillingCountry}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nShipping Address<br />\r\n{{Order.ShippingFirstName}} {{Order.ShippingLastName}}<br />\r\n{{Order.ShippingAddress1}}<br />\r\n{{Order.ShippingCity}} {{Order.ShippingZipPostalCode}}<br />\r\n{{Order.ShippingStateProvince}} {{Order.ShippingCountry}}<br />\r\n<br />\r\nShipping Method: {{Order.ShippingMethod}}<br />\r\n<br />\r\n" + OrderNotes + "</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                    new MessageTemplate
                                       {
                                           Name = "OrderCancelled.VendorNotification",
                                           Subject = "{{Store.Name}}. Order #{{Order.OrderNumber}} cancelled",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br /><br />Order #{{Order.OrderNumber}} has been cancelled. <br /><br />Order Number: {{Order.OrderNumber}} <br />   Date Ordered: {{Order.CreatedOn}} <br /><br /> ",
                                           IsActive = false,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                    new MessageTemplate
                                       {
                                           Name = "OrderCompleted.UserNotification",
                                           Subject = "{{Store.Name}}. Your order completed",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nHello {{Order.UserFullName}}, <br />\r\nYour order has been completed. Below is the summary of the order. <br />\r\n<br />\r\nOrder Number: {{Order.OrderNumber}}<br />\r\nOrder Details: <a target=\"_blank\" href=\"{{Order.OrderURLForUser}}\">{{Order.OrderURLForUser}}</a><br />\r\nDate Ordered: {{Order.CreatedOn}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nBilling Address<br />\r\n{{Order.BillingFirstName}} {{Order.BillingLastName}}<br />\r\n{{Order.BillingAddress1}}<br />\r\n{{Order.BillingCity}} {{Order.BillingZipPostalCode}}<br />\r\n{{Order.BillingStateProvince}} {{Order.BillingCountry}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nShipping Address<br />\r\n{{Order.ShippingFirstName}} {{Order.ShippingLastName}}<br />\r\n{{Order.ShippingAddress1}}<br />\r\n{{Order.ShippingCity}} {{Order.ShippingZipPostalCode}}<br />\r\n{{Order.ShippingStateProvince}} {{Order.ShippingCountry}}<br />\r\n<br />\r\nShipping Method: {{Order.ShippingMethod}}<br />\r\n<br />\r\n" + OrderNotes + "</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "ShipmentDelivered.UserNotification",
                                           Subject = "Your order from {{Store.Name}} has been delivered.",
                                           Body = "<p><a href=\"{{Store.URL}}\"> {{Store.Name}}</a> <br />\r\n <br />\r\n Hello {{Order.UserFullName}}, <br />\r\n Good news! You order has been delivered. <br />\r\n Order Number: {{Order.OrderNumber}}<br />\r\n Order Details: <a href=\"{{Order.OrderURLForUser}}\" target=\"_blank\">{{Order.OrderURLForUser}}</a><br />\r\n Date Ordered: {{Order.CreatedOn}}<br />\r\n <br />\r\n <br />\r\n <br />\r\n Billing Address<br />\r\n {{Order.BillingFirstName}} {{Order.BillingLastName}}<br />\r\n {{Order.BillingAddress1}}<br />\r\n {{Order.BillingCity}} {{Order.BillingZipPostalCode}}<br />\r\n {{Order.BillingStateProvince}} {{Order.BillingCountry}}<br />\r\n <br />\r\n <br />\r\n <br />\r\n Shipping Address<br />\r\n {{Order.ShippingFirstName}} {{Order.ShippingLastName}}<br />\r\n {{Order.ShippingAddress1}}<br />\r\n {{Order.ShippingCity}} {{Order.ShippingZipPostalCode}}<br />\r\n {{Order.ShippingStateProvince}} {{Order.ShippingCountry}}<br />\r\n <br />\r\n Shipping Method: {{Order.ShippingMethod}} <br />\r\n <br />\r\n Delivered Notes: <br />\r\n <br />\r\n" + ShipmentNotes + "</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "OrderPlaced.UserNotification",
                                           Subject = "Order receipt from {{Store.Name}}.",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nHello {{Order.UserFullName}}, <br />\r\nThanks for buying from <a href=\"{{Store.URL}}\">{{Store.Name}}</a>. Below is the summary of the order. <br />\r\n<br />\r\nOrder Number: {{Order.OrderNumber}}<br />\r\nOrder Details: <a target=\"_blank\" href=\"{{Order.OrderURLForUser}}\">{{Order.OrderURLForUser}}</a><br />\r\nDate Ordered: {{Order.CreatedOn}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nBilling Address<br />\r\n{{Order.BillingFirstName}} {{Order.BillingLastName}}<br />\r\n{{Order.BillingAddress1}}<br />\r\n{{Order.BillingCity}} {{Order.BillingZipPostalCode}}<br />\r\n{{Order.BillingStateProvince}} {{Order.BillingCountry}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nShipping Address<br />\r\n{{Order.ShippingFirstName}} {{Order.ShippingLastName}}<br />\r\n{{Order.ShippingAddress1}}<br />\r\n{{Order.ShippingCity}} {{Order.ShippingZipPostalCode}}<br />\r\n{{Order.ShippingStateProvince}} {{Order.ShippingCountry}}<br />\r\n<br />\r\nShipping Method: {{Order.ShippingMethod}}<br />\r\n<br />\r\n" + OrderNotes + "</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "OrderPlaced.StoreOwnerNotification",
                                           Subject = "{{Store.Name}}. Purchase Receipt for Order #{{Order.OrderNumber}}",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\n{{Order.UserFullName}} ({{Order.UserEmail}}) has just placed an order from your store. Below is the summary of the order. <br />\r\n<br />\r\nOrder Number: {{Order.OrderNumber}}<br />\r\nDate Ordered: {{Order.CreatedOn}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nBilling Address<br />\r\n{{Order.BillingFirstName}} {{Order.BillingLastName}}<br />\r\n{{Order.BillingAddress1}}<br />\r\n{{Order.BillingCity}} {{Order.BillingZipPostalCode}}<br />\r\n{{Order.BillingStateProvince}} {{Order.BillingCountry}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nShipping Address<br />\r\n{{Order.ShippingFirstName}} {{Order.ShippingLastName}}<br />\r\n{{Order.ShippingAddress1}}<br />\r\n{{Order.ShippingCity}} {{Order.ShippingZipPostalCode}}<br />\r\n{{Order.ShippingStateProvince}} {{Order.ShippingCountry}}<br />\r\n<br />\r\nShipping Method: {{Order.ShippingMethod}}<br />\r\n<br />\r\n" + OrderNotes + "</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "ShipmentSent.UserNotification",
                                           Subject = "Your order from {{Store.Name}} has been shipped.",
                                           Body = "<p><a href=\"{{Store.URL}}\"> {{Store.Name}}</a> <br />\r\n<br />\r\nHello {{Order.UserFullName}}!, <br />\r\nGood news! You order has been shipped. <br />\r\nOrder Number: {{Order.OrderNumber}}<br />\r\nOrder Details: <a href=\"{{Order.OrderURLForUser}}\" target=\"_blank\">{{Order.OrderURLForUser}}</a><br />\r\nDate Ordered: {{Order.CreatedOn}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nBilling Address<br />\r\n{{Order.BillingFirstName}} {{Order.BillingLastName}}<br />\r\n{{Order.BillingAddress1}}<br />\r\n{{Order.BillingCity}} {{Order.BillingZipPostalCode}}<br />\r\n{{Order.BillingStateProvince}} {{Order.BillingCountry}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nShipping Address<br />\r\n{{Order.ShippingFirstName}} {{Order.ShippingLastName}}<br />\r\n{{Order.ShippingAddress1}}<br />\r\n{{Order.ShippingCity}} {{Order.ShippingZipPostalCode}}<br />\r\n{{Order.ShippingStateProvince}} {{Order.ShippingCountry}}<br />\r\n<br />\r\nShipping Method: {{Order.ShippingMethod}} <br />\r\n <br />\r\n Shipped Notes: <br />\r\n <br />\r\n" + ShipmentNotes + "</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "Note.NoteReview",
                                           Subject = "{{Store.Name}}. New note review.",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nA new note review has been written for note \"{{NoteReview.NoteName}}\".</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "QuantityBelow.StoreOwnerNotification",
                                           Subject = "{{Store.Name}}. Quantity below notification. {{Note.Name}}",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\n{{Note.Name}} (ID: {{Note.Id}}) low quantity. <br />\r\n<br />\r\nQuantity: {{Note.StockQuantity}}<br />\r\n</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "QuantityBelow.AttributeCombination.StoreOwnerNotification",
                                           Subject = "{{Store.Name}}. Quantity below notification. {{Note.Name}}",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\n{{Note.Name}} (ID: {{Note.Id}}) low quantity. <br />\r\n{{AttributeCombination.Formatted}}<br />\r\nQuantity: {{AttributeCombination.StockQuantity}}<br />\r\n</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "ReturnRequestStatusChanged.UserNotification",
                                           Subject = "{{Store.Name}}. Return request status was changed.",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nHello {{User.FullName}},<br />\r\nYour return request #{{ReturnRequest.Id}} status has been changed.</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "Service.EmailAFriend",
                                           Subject = "{{Store.Name}}. Referred Item",
                                           Body = "<p><a href=\"{{Store.URL}}\"> {{Store.Name}}</a> <br />\r\n<br />\r\n{{EmailAFriend.Email}} was shopping on {{Store.Name}} and wanted to share the following item with you. <br />\r\n<br />\r\n<b><a target=\"_blank\" href=\"{{Note.NoteURLForUser}}\">{{Note.Name}}</a></b> <br />\r\n{{Note.ShortDescription}} <br />\r\n<br />\r\nFor more info click <a target=\"_blank\" href=\"{{Note.NoteURLForUser}}\">here</a> <br />\r\n<br />\r\n<br />\r\n{{EmailAFriend.PersonalMessage}}<br />\r\n<br />\r\n{{Store.Name}}</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "Service.AskQuestion",
                                           Subject = "{{Store.Name}}. Question about a note",
                                           Body = "<p><a href=\"{{Store.URL}}\"> {{Store.Name}}</a> <br />\r\n<br />\r\n{{AskQuestion.Email}} wanted to ask question about a note {{Note.Name}}. <br />\r\n<br />\r\n<b><a target=\"_blank\" href=\"{{Note.NoteURLForUser}}\">{{Note.Name}}</a></b> <br />\r\n{{Note.ShortDescription}} <br />\r\n{{AskQuestion.Message}}<br />\r\n {{AskQuestion.Email}} <br />\r\n {{AskQuestion.FullName}} <br />\r\n {{AskQuestion.Phone}} <br />\r\n{{Store.Name}}</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "Service.ContactUs",
                                           Subject = "{{Store.Name}}. Contact us",
                                           Body = "<p>From {{ContactUs.SenderName}} - {{ContactUs.SenderEmail}}<br /><br />{{ContactUs.Body}}<br />{{ContactUs.AttributeDescription}}</p><br />",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "Service.ContactVendor",
                                           Subject = "{{Store.Name}}. Contact us",
                                           Body = "<p>From {{ContactUs.SenderName}} - {{ContactUs.SenderEmail}}<br /><br />{{ContactUs.Body}}</p><br />",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },

                                   new MessageTemplate
                                       {
                                           Name = "Wishlist.EmailAFriend",
                                           Subject = "{{Store.Name}}. Wishlist",
                                           Body = "<p><a href=\"{{Store.URL}}\"> {{Store.Name}}</a> <br />\r\n<br />\r\n{{ShoppingCart.WishlistEmail}} was shopping on {{Store.Name}} and wanted to share a wishlist with you. <br />\r\n<br />\r\n<br />\r\nFor more info click <a target=\"_blank\" href=\"{{ShoppingCart.WishlistURLForUser}}\">here</a> <br />\r\n<br />\r\n<br />\r\n{{ShoppingCart.WishlistPersonalMessage}}<br />\r\n<br />\r\n{{Store.Name}}</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "User.NewOrderNote",
                                           Subject = "{{Store.Name}}. New order note has been added",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nHello {{User.FullName}}, <br />\r\nNew order note has been added to your account:<br />\r\n\"{{Order.NewNoteText}}\".<br />\r\n<a target=\"_blank\" href=\"{{Order.OrderURLForUser}}\">{{Order.OrderURLForUser}}</a></p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "User.NewUserNote",
                                           Subject = "New user note has been added",
                                           Body = "<p><br />\r\nHello {{User.FullName}}, <br />\r\nNew user note has been added to your account:<br />\r\n\"{{User.NewTitleText}}\".<br />\r\n</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                     new MessageTemplate
                                       {
                                           Name = "User.NewReturnRequestNote",
                                           Subject = "{{Store.Name}}. New return request note has been added",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nHello {{User.FullName}},<br />\r\nYour return request #{{ReturnRequest.ReturnNumber}} has a new note.</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "RecurringPaymentCancelled.StoreOwnerNotification",
                                           Subject = "{{Store.Name}}. Recurring payment cancelled",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\n{{User.FullName}} ({{User.Email}}) has just cancelled a recurring payment ID={{RecurringPayment.ID}}.</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "OrderPlaced.VendorNotification",
                                           Subject = "{{Store.Name}}. Order placed",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\n{{User.FullName}} ({{User.Email}}) has just placed an order. <br />\r\n<br />\r\nOrder Number: {{Order.OrderNumber}}<br />\r\nDate Ordered: {{Order.CreatedOn}}<br />\r\n<br />\r\n" + OrderVendorNotes + "</p>",
                                           //this template is disabled by default
                                           IsActive = false,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "OrderPaid.StoreOwnerNotification",
                                           Subject = "{{Store.Name}}. Order #{{Order.OrderNumber}} paid",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nOrder #{{Order.OrderNumber}} has been just paid<br />\r\nDate Ordered: {{Order.CreatedOn}}</p>",
                                           //this template is disabled by default
                                           IsActive = false,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "OrderPaid.UserNotification",
                                           Subject = "{{Store.Name}}. Order #{{Order.OrderNumber}} paid",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nHello {{Order.UserFullName}}, <br />\r\nThanks for buying from <a href=\"{{Store.URL}}\">{{Store.Name}}</a>. Order #{{Order.OrderNumber}} has been just paid. Below is the summary of the order. <br />\r\n<br />\r\nOrder Number: {{Order.OrderNumber}}<br />\r\nOrder Details: <a href=\"{{Order.OrderURLForUser}}\" target=\"_blank\">{{Order.OrderURLForUser}}</a><br />\r\nDate Ordered: {{Order.CreatedOn}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nBilling Address<br />\r\n{{Order.BillingFirstName}} {{Order.BillingLastName}}<br />\r\n{{Order.BillingAddress1}}<br />\r\n{{Order.BillingCity}} {{Order.BillingZipPostalCode}}<br />\r\n{{Order.BillingStateProvince}} {{Order.BillingCountry}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nShipping Address<br />\r\n{{Order.ShippingFirstName}} {{Order.ShippingLastName}}<br />\r\n{{Order.ShippingAddress1}}<br />\r\n{{Order.ShippingCity}} {{Order.ShippingZipPostalCode}}<br />\r\n{{Order.ShippingStateProvince}} {{Order.ShippingCountry}}<br />\r\n<br />\r\nShipping Method: {{Order.ShippingMethod}}<br />\r\n<br />\r\n" + OrderNotes + "</p>",
                                           //this template is disabled by default
                                           IsActive = false,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                       {
                                           Name = "OrderPaid.VendorNotification",
                                           Subject = "{{Store.Name}}. Order #{{Order.OrderNumber}} paid",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nOrder #{{Order.OrderNumber}} has been just paid. <br />\r\n<br />\r\nOrder Number: {{Order.OrderNumber}}<br />\r\nDate Ordered: {{Order.CreatedOn}}<br />\r\n<br />\r\n" + OrderVendorNotes + "</p>",
                                           //this template is disabled by default
                                           IsActive = false,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                   new MessageTemplate
                                        {
                                           Name = "OrderRefunded.UserNotification",
                                           Subject = "{{Store.Name}}. Order #{{Order.OrderNumber}} refunded",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nHello {{Order.UserFullName}}, <br />\r\nThanks for buying from <a href=\"{{Store.URL}}\">{{Store.Name}}</a>. Order #{{Order.OrderNumber}} has been has been refunded. Please allow 7-14 days for the refund to be reflected in your account.<br />\r\n<br />\r\nAmount refunded: {{Order.AmountRefunded}}<br />\r\n<br />\r\nBelow is the summary of the order. <br />\r\n<br />\r\nOrder Number: {{Order.OrderNumber}}<br />\r\nOrder Details: <a href=\"{{Order.OrderURLForUser}}\" target=\"_blank\">{{Order.OrderURLForUser}}</a><br />\r\nDate Ordered: {{Order.CreatedOn}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nBilling Address<br />\r\n{{Order.BillingFirstName}} {{Order.BillingLastName}}<br />\r\n{{Order.BillingAddress1}}<br />\r\n{{Order.BillingCity}} {{Order.BillingZipPostalCode}}<br />\r\n{{Order.BillingStateProvince}} {{Order.BillingCountry}}<br />\r\n<br />\r\n<br />\r\n<br />\r\nShipping Address<br />\r\n{{Order.ShippingFirstName}} {{Order.ShippingLastName}}<br />\r\n{{Order.ShippingAddress1}}<br />\r\n{{Order.ShippingCity}} {{Order.ShippingZipPostalCode}}<br />\r\n{{Order.ShippingStateProvince}} {{Order.ShippingCountry}}<br />\r\n<br />\r\nShipping Method: {{Order.ShippingMethod}}<br />\r\n<br />\r\n" + OrderNotes + "</p>",
                                           //this template is disabled by default
                                           IsActive = false,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                    new MessageTemplate
                                        {
                                           Name = "OrderRefunded.StoreOwnerNotification",
                                           Subject = "{{Store.Name}}. Order #{{Order.OrderNumber}} refunded",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nOrder #{{Order.OrderNumber}} has been just refunded<br />\r\n<br />\r\nAmount refunded: {{Order.AmountRefunded}}<br />\r\n<br />\r\nDate Ordered: {{Order.CreatedOn}}</p>",
                                           //this template is disabled by default
                                           IsActive = false,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                    new MessageTemplate
                                       {
                                           Name = "VendorAccountApply.StoreOwnerNotification",
                                           Subject = "{{Store.Name}}. New vendor account submitted.",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\n{{User.FullName}} ({{User.Email}}) has just submitted for a vendor account. Details are below:<br />\r\nVendor name: {{Vendor.Name}}<br />\r\nVendor email: {{Vendor.Email}}<br />\r\n<br />\r\nYou can activate it in admin area.</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                    new MessageTemplate
                                       {
                                           Name = "Vendor.VendorReview",
                                           Subject = "{{Store.Name}}. New vendor review.",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\nA new vendor review has been written.</p>",
                                           IsActive = true,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                                    new MessageTemplate
                                       { 
                                           Name = "VendorInformationChange.StoreOwnerNotification",
                                           Subject = "{{Store.Name}}. Vendor {{Vendor.Name}} changed provided information",
                                           Body = "<p><a href=\"{{Store.URL}}\">{{Store.Name}}</a> <br />\r\n<br />\r\n{{Vendor.Name}} changed provided information.</p>",
                                           IsActive = false,
                                           EmailAccountId = eaGeneral.Id,
                                       },
                               };
            await _messageTemplateRepository.InsertAsync(messageTemplates);
        }

        protected virtual async Task InstallSettings()
        {
            var _settingService = _serviceProvider.GetRequiredService<ISettingService>();

            await _settingService.SaveSetting(new MenuItemSettings {
            });

            await _settingService.SaveSetting(new CommonSettings {
                StoreInDatabaseContactUsForm = true,
                UseSystemEmailForContactUsForm = true,
                UseStoredProceduresIfSupported = true,
                DisplayJavaScriptDisabledWarning = false,
                UseFullTextSearch = false,
                FullTextMode = FulltextSearchMode.ExactMatch,
                Log404Errors = true,
                BreadcrumbDelimiter = "/",
                RenderXuaCompatible = false,
                XuaCompatibleValue = "IE=edge",
                DeleteGuestTaskOlderThanMinutes = 1440,
                PopupForTermsOfServiceLinks = true,
                AllowToSelectStore = false,
            });
            await _settingService.SaveSetting(new SecuritySettings {
                EncryptionKey = CommonHelper.GenerateRandomDigitCode(24),
                AdminAreaAllowedIpAddresses = null,
                EnableXsrfProtectionForAdminArea = true,
                EnableXsrfProtectionForPublicStore = true,
                HoneypotEnabled = false,
                HoneypotInputName = "hpinput",
                AllowNonAsciiCharInHeaders = true,
            });
            await _settingService.SaveSetting(new MediaSettings {
                AvatarPictureSize = 120,
                NoteThumbPictureSize = 415,
                NoteDetailsPictureSize = 550,
                NoteThumbPictureSizeOnNoteDetailsPage = 100,
                AssociatedNotePictureSize = 220,
                NotebookThumbPictureSize = 450,
                AutoCompleteSearchThumbPictureSize = 50,
                ImageSquarePictureSize = 32,
                MaximumImageSize = 1980,
                DefaultPictureZoomEnabled = true,
                DefaultImageQuality = 80,
                MultipleThumbDirectories = false,
                StoreLocation = "/",
                StoreInDb = true
            });

            await _settingService.SaveSetting(new AdminAreaSettings {
                DefaultGridPageSize = 15,
                GridPageSizes = "10, 15, 20, 50, 100",
                RichEditorAdditionalSettings = null,
                RichEditorAllowJavaScript = false,
                UseIsoDateTimeConverterInJson = true,
            });

            await _settingService.SaveSetting(new CatalogSettings {
                AllowNoteSorting = true,
                DefaultViewMode = "grid",
                ShowNotesFromSubnotebooks = true,
                ShowNotebookNoteCount = false,
                ShowNotebookNoteNumberIncludingSubnotebooks = false,
                NotebookBreadcrumbEnabled = true,
                ShowShareButton = false,
                RecentlyViewedNotesNumber = 3,
                RecentlyViewedNotesEnabled = true,
                NewNotesNumber = 6,
                NewNotesEnabled = true,
                NewNotesOnHomePage = false,
                NewNotesNumberOnHomePage = 6,
                NoteSearchAutoCompleteEnabled = true,
                NoteSearchAutoCompleteNumberOfNotes = 10,
                NoteSearchTermMinimumLength = 3,
                ShowNoteImagesInSearchAutoComplete = true,
                SearchPageNotesPerPage = 6,
                SearchPageAllowUsersToSelectPageSize = true,
                SearchPagePageSizeOptions = "6, 3, 9, 18",
                NumberOfNoteTags = 15,
                NotesByTagPageSize = 6,
                IncludeFeaturedNotesInNormalLists = false,
                IgnoreFeaturedNotes = false,
                NotesByTagAllowUsersToSelectPageSize = true,
                NotesByTagPageSizeOptions = "6, 3, 9, 18",
                DefaultNotebookPageSizeOptions = "6, 3, 9",
                LimitOfFeaturedNotes = 30,
            });

            await _settingService.SaveSetting(new LocalizationSettings {
                DefaultAdminLanguageId = _languageRepository.Table.Single(l => l.Name == "English").Id,
                UseImagesForLanguageSelection = false,
                AutomaticallyDetectLanguage = false,
                LoadAllLocaleRecordsOnStartup = true,
                LoadAllLocalizedPropertiesOnStartup = true,
                LoadAllUrlRecordsOnStartup = false,
                IgnoreRtlPropertyForAdminArea = false,
            });

            await _settingService.SaveSetting(new UserSettings {
                UsernamesEnabled = false,
                CheckUsernameAvailabilityEnabled = false,
                AllowUsersToChangeUsernames = false,
                DefaultPasswordFormat = PasswordFormat.Hashed,
                HashedPasswordFormat = "SHA1",
                PasswordMinLength = 6,
                PasswordRecoveryLinkDaysValid = 7,
                PasswordLifetime = 90,
                FailedPasswordAllowedAttempts = 0,
                FailedPasswordLockoutMinutes = 30,
                UserRegistrationType = UserRegistrationType.Standard,
                AllowUsersToUploadAvatars = false,
                AvatarMaximumSizeBytes = 20000,
                DefaultAvatarEnabled = true,
                UserNameFormat = UserNameFormat.ShowFirstName,
                TwoFactorAuthenticationEnabled = false,
            });

            await _settingService.SaveSetting(new StoreInformationSettings {
                StoreClosed = false,
                DefaultStoreTheme = "DefaultClean",
                AllowUserToSelectTheme = false,
                DisplayEuCookieLawWarning = false,
                FacebookLink = "https://www.facebook.com/forevernotecom",
                TwitterLink = "https://twitter.com/forevernote",
                YoutubeLink = "http://www.youtube.com/user/forevernote",
                InstagramLink = "https://www.instagram.com/forevernote/",
                LinkedInLink = "https://www.linkedin.com/company/forever-note.com/",
                PinterestLink = "",
                HidePoweredByForeverNote = false
            });

            await _settingService.SaveSetting(new ExternalAuthenticationSettings {
                AutoRegisterEnabled = true,
                RequireEmailValidation = false
            });

            await _settingService.SaveSetting(new MessageTemplatesSettings {
                CaseInvariantReplacement = false,
                Color1 = "#b9babe",
                Color2 = "#ebecee",
                Color3 = "#dde2e6",
                PictureSize = 50,
            });

            await _settingService.SaveSetting(new DateTimeSettings {
                DefaultStoreTimeZoneId = "",
                AllowUsersToSetTimeZone = false
            });

            await _settingService.SaveSetting(new PushNotificationsSettings {
                Enabled = false,
                AllowGuestNotifications = true
            });

            await _settingService.SaveSetting(new AdminSearchSettings {
                NotebooksDisplayOrder = 0,
                MaxSearchResultsCount = 10,
                MinSearchTermLength = 3,
                NotesDisplayOrder = 0,
                SearchInNotebooks = true,
                SearchInNotes = true,
                SearchInMenu = true,
                MenuDisplayOrder = -1
            });

            var eaGeneral = _emailAccountRepository.Table.FirstOrDefault();
            if (eaGeneral == null)
                throw new Exception("Default email account cannot be loaded");
            await _settingService.SaveSetting(new EmailAccountSettings {
                DefaultEmailAccountId = eaGeneral.Id
            });
        }

        protected virtual async Task InstallNotebooks()
        {
            var pictureService = _serviceProvider.GetRequiredService<IPictureService>();

            //sample pictures
            var sampleImagesPath = GetSamplesPath();

            //notebooks
            var allNotebooks = new List<Notebook>();
            var notebookComputers = new Notebook {
                Name = "Computers",
                PageSize = 6,
                ParentNotebookId = "",
                PictureId = (await pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "notebook_computers.jpeg"), "image/jpeg", "Computers")).Id,
                Flag = "New",
                FlagStyle = "badge-danger",
                DisplayOrder = 1,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allNotebooks.Add(notebookComputers);

            await _notebookRepository.InsertAsync(allNotebooks);
        }

        protected virtual async Task InstallActivityLogTypes()
        {
            var activityLogTypes = new List<ActivityLogType>
                                      {
                                          //admin area activities
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewNotebook",
                                                  Enabled = true,
                                                  Name = "Add a new notebook"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewCheckoutAttribute",
                                                  Enabled = true,
                                                  Name = "Add a new checkout attribute"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewContactAttribute",
                                                  Enabled = true,
                                                  Name = "Add a new contact attribute"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewUser",
                                                  Enabled = true,
                                                  Name = "Add a new user"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewUserRole",
                                                  Enabled = true,
                                                  Name = "Add a new user role"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewDiscount",
                                                  Enabled = true,
                                                  Name = "Add a new discount"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewDocument",
                                                  Enabled = false,
                                                  Name = "Add a new document"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewDocumentType",
                                                  Enabled = false,
                                                  Name = "Add a new document type"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewGiftCard",
                                                  Enabled = true,
                                                  Name = "Add a new gift card"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewManufacturer",
                                                  Enabled = true,
                                                  Name = "Add a new manufacturer"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewNote",
                                                  Enabled = true,
                                                  Name = "Add a new note"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewNoteAttribute",
                                                  Enabled = true,
                                                  Name = "Add a new note attribute"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewSetting",
                                                  Enabled = true,
                                                  Name = "Add a new setting"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewSpecAttribute",
                                                  Enabled = true,
                                                  Name = "Add a new specification attribute"
                                              },
                                          new ActivityLogType
                                              {
                                                SystemKeyword = "AddNewTopic",
                                                Enabled = true,
                                                Name = "Add a new topic"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "AddNewWidget",
                                                  Enabled = true,
                                                  Name = "Add a new widget"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteBid",
                                                  Enabled = true,
                                                  Name = "Delete bid"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteNotebook",
                                                  Enabled = true,
                                                  Name = "Delete notebook"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteCheckoutAttribute",
                                                  Enabled = true,
                                                  Name = "Delete a checkout attribute"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteContactAttribute",
                                                  Enabled = true,
                                                  Name = "Delete a contact attribute"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteUser",
                                                  Enabled = true,
                                                  Name = "Delete a user"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteUserRole",
                                                  Enabled = true,
                                                  Name = "Delete a user role"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteDiscount",
                                                  Enabled = true,
                                                  Name = "Delete a discount"
                                              },

                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteDocument",
                                                  Enabled = false,
                                                  Name = "Delete document"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteDocumentType",
                                                  Enabled = false,
                                                  Name = "Delete document type"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteGiftCard",
                                                  Enabled = true,
                                                  Name = "Delete a gift card"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteManufacturer",
                                                  Enabled = true,
                                                  Name = "Delete a manufacturer"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteOrder",
                                                  Enabled = true,
                                                  Name = "Delete an order"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteNote",
                                                  Enabled = true,
                                                  Name = "Delete a note"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteNoteAttribute",
                                                  Enabled = true,
                                                  Name = "Delete a note attribute"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteReturnRequest",
                                                  Enabled = true,
                                                  Name = "Delete a return request"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteSetting",
                                                  Enabled = true,
                                                  Name = "Delete a setting"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteSpecAttribute",
                                                  Enabled = true,
                                                  Name = "Delete a specification attribute"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteTopic",
                                                  Enabled = true,
                                                  Name = "Delete a topic"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteWidget",
                                                  Enabled = true,
                                                  Name = "Delete a widget"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditNotebook",
                                                  Enabled = true,
                                                  Name = "Edit notebook"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditCheckoutAttribute",
                                                  Enabled = true,
                                                  Name = "Edit a checkout attribute"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditContactAttribute",
                                                  Enabled = true,
                                                  Name = "Edit a contact attribute"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditUser",
                                                  Enabled = true,
                                                  Name = "Edit a user"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditUserRole",
                                                  Enabled = true,
                                                  Name = "Edit a user role"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditDiscount",
                                                  Enabled = true,
                                                  Name = "Edit a discount"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditDocument",
                                                  Enabled = false,
                                                  Name = "Edit document"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditDocumentType",
                                                  Enabled = false,
                                                  Name = "Edit document type"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditGiftCard",
                                                  Enabled = true,
                                                  Name = "Edit a gift card"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditManufacturer",
                                                  Enabled = true,
                                                  Name = "Edit a manufacturer"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditOrder",
                                                  Enabled = true,
                                                  Name = "Edit an order"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditNote",
                                                  Enabled = true,
                                                  Name = "Edit a note"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditNoteAttribute",
                                                  Enabled = true,
                                                  Name = "Edit a note attribute"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditPromotionProviders",
                                                  Enabled = true,
                                                  Name = "Edit promotion providers"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditReturnRequest",
                                                  Enabled = true,
                                                  Name = "Edit a return request"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditSettings",
                                                  Enabled = true,
                                                  Name = "Edit setting(s)"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditSpecAttribute",
                                                  Enabled = true,
                                                  Name = "Edit a specification attribute"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "EditTopic",
                                                  Enabled = true,
                                                  Name = "Edit a topic"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "InteractiveFormDelete",
                                                  Enabled = true,
                                                  Name = "Delete a interactive form"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "InteractiveFormEdit",
                                                  Enabled = true,
                                                  Name = "Edit a interactive form"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "InteractiveFormAdd",
                                                  Enabled = true,
                                                  Name = "Add a interactive form"
                                              },
                                           new ActivityLogType
                                              {
                                                  SystemKeyword = "EditWidget",
                                                  Enabled = true,
                                                  Name = "Edit a widget"
                                              },
                                              //public store activities
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.Url",
                                                  Enabled = false,
                                                  Name = "Public store. Viewed Url"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.ViewNotebook",
                                                  Enabled = false,
                                                  Name = "Public store. View a notebook"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.ViewManufacturer",
                                                  Enabled = false,
                                                  Name = "Public store. View a manufacturer"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.ViewNote",
                                                  Enabled = false,
                                                  Name = "Public store. View a note"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.ViewCourse",
                                                  Enabled = false,
                                                  Name = "Public store. View a course"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.ViewLesson",
                                                  Enabled = false,
                                                  Name = "Public store. View a lesson"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.AskQuestion",
                                                  Enabled = false,
                                                  Name = "Public store. Ask a question about note"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.InteractiveForm",
                                                  Enabled = false,
                                                  Name = "Public store. Show interactive form"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.PlaceOrder",
                                                  Enabled = false,
                                                  Name = "Public store. Place an order"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.SendPM",
                                                  Enabled = false,
                                                  Name = "Public store. Send PM"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.ContactUs",
                                                  Enabled = false,
                                                  Name = "Public store. Use contact us form"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.AddNewBid",
                                                  Enabled = false,
                                                  Name = "Public store. Add new bid"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.AddToCompareList",
                                                  Enabled = false,
                                                  Name = "Public store. Add to compare list"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.AddToShoppingCart",
                                                  Enabled = false,
                                                  Name = "Public store. Add to shopping cart"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.AddToWishlist",
                                                  Enabled = false,
                                                  Name = "Public store. Add to wishlist"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.Login",
                                                  Enabled = false,
                                                  Name = "Public store. Login"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.Logout",
                                                  Enabled = false,
                                                  Name = "Public store. Logout"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.AddNoteReview",
                                                  Enabled = false,
                                                  Name = "Public store. Add note review"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.AddNewsComment",
                                                  Enabled = false,
                                                  Name = "Public store. Add news comment"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.AddBlogComment",
                                                  Enabled = false,
                                                  Name = "Public store. Add blog comment"
                                              },
                                        new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.AddArticleComment",
                                                  Enabled = false,
                                                  Name = "Public store. Add article comment"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.AddForumTopic",
                                                  Enabled = false,
                                                  Name = "Public store. Add forum topic"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.EditForumTopic",
                                                  Enabled = false,
                                                  Name = "Public store. Edit forum topic"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.DeleteForumTopic",
                                                  Enabled = false,
                                                  Name = "Public store. Delete forum topic"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.AddForumPost",
                                                  Enabled = false,
                                                  Name = "Public store. Add forum post"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.EditForumPost",
                                                  Enabled = false,
                                                  Name = "Public store. Edit forum post"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.DeleteForumPost",
                                                  Enabled = false,
                                                  Name = "Public store. Delete forum post"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "PublicStore.DeleteAccount",
                                                  Enabled = false,
                                                  Name = "Public store. Delete account"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "UserReminder.AbandonedCart",
                                                  Enabled = true,
                                                  Name = "Send email User reminder - AbandonedCart"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "UserReminder.RegisteredUser",
                                                  Enabled = true,
                                                  Name = "Send email User reminder - RegisteredUser"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "UserReminder.LastActivity",
                                                  Enabled = true,
                                                  Name = "Send email User reminder - LastActivity"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "UserReminder.LastPurchase",
                                                  Enabled = true,
                                                  Name = "Send email User reminder - LastPurchase"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "UserReminder.Birthday",
                                                  Enabled = true,
                                                  Name = "Send email User reminder - Birthday"
                                              },
                                          new ActivityLogType
                                              {
                                                  SystemKeyword = "UserReminder.SendCampaign",
                                                  Enabled = true,
                                                  Name = "Send Campaign"
                                              },
                                           new ActivityLogType
                                              {
                                                  SystemKeyword = "UserAdmin.SendEmail",
                                                  Enabled = true,
                                                  Name = "Send email"
                                              },
                                            new ActivityLogType
                                              {
                                                  SystemKeyword = "UserAdmin.SendPM",
                                                  Enabled = true,
                                                  Name = "Send PM"
                                              },
                                            new ActivityLogType
                                              {
                                                  SystemKeyword = "UpdateKnowledgebaseNotebook",
                                                  Enabled = true,
                                                  Name = "Update knowledgebase notebook"
                                              },
                                            new ActivityLogType
                                              {
                                                  SystemKeyword = "CreateKnowledgebaseNotebook",
                                                  Enabled = true,
                                                  Name = "Create knowledgebase notebook"
                                              },
                                            new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteKnowledgebaseNotebook",
                                                  Enabled = true,
                                                  Name = "Delete knowledgebase notebook"
                                              },
                                            new ActivityLogType
                                              {
                                                  SystemKeyword = "CreateKnowledgebaseArticle",
                                                  Enabled = true,
                                                  Name = "Create knowledgebase article"
                                              },
                                            new ActivityLogType
                                              {
                                                  SystemKeyword = "UpdateKnowledgebaseArticle",
                                                  Enabled = true,
                                                  Name = "Update knowledgebase article"
                                              },
                                            new ActivityLogType
                                              {
                                                  SystemKeyword = "DeleteKnowledgebaseArticle",
                                                  Enabled = true,
                                                  Name = "Delete knowledgebase notebook"
                                              },
                                      };
            await _activityLogTypeRepository.InsertAsync(activityLogTypes);
        }

        protected virtual async Task InstallScheduleTasks()
        {
            //these tasks are default - they are created in order to insert them into database
            //and nothing above it
            //there is no need to send arguments into ctor - all are null
            var tasks = new List<ScheduleTask>
            {
            new ScheduleTask
            {
                    ScheduleTaskName = "Send emails",
                    Type = "ForeverNote.Services.Tasks.QueuedMessagesSendScheduleTask, ForeverNote.Services",
                    Enabled = true,
                    StopOnError = false,
                    TimeInterval = 1
                },
                new ScheduleTask
                {
                    ScheduleTaskName = "Delete guests",
                    Type = "ForeverNote.Services.Tasks.DeleteGuestsScheduleTask, ForeverNote.Services",
                    Enabled = true,
                    StopOnError = false,
                    TimeInterval = 1440
                },
                new ScheduleTask
                {
                    ScheduleTaskName = "Clear cache",
                    Type = "ForeverNote.Services.Tasks.ClearCacheScheduleTask, ForeverNote.Services",
                    Enabled = false,
                    StopOnError = false,
                    TimeInterval = 120
                },
                new ScheduleTask
                {
                    ScheduleTaskName = "Clear log",
                    Type = "ForeverNote.Services.Tasks.ClearLogScheduleTask, ForeverNote.Services",
                    Enabled = false,
                    StopOnError = false,
                    TimeInterval = 1440
                },
                new ScheduleTask
                {
                    ScheduleTaskName = "Update currency exchange rates",
                    Type = "ForeverNote.Services.Tasks.UpdateExchangeRateScheduleTask, ForeverNote.Services",
                    Enabled = true,
                    StopOnError = false,
                    TimeInterval = 1440
                },
                new ScheduleTask
                {
                    ScheduleTaskName = "User reminder - AbandonedCart",
                    Type = "ForeverNote.Services.Tasks.UserReminderAbandonedCartScheduleTask, ForeverNote.Services",
                    Enabled = false,
                    StopOnError = false,
                    TimeInterval = 20
                },
                new ScheduleTask
                {
                    ScheduleTaskName = "User reminder - RegisteredUser",
                    Type = "ForeverNote.Services.Tasks.UserReminderRegisteredUserScheduleTask, ForeverNote.Services",
                    Enabled = false,
                    StopOnError = false,
                    TimeInterval = 1440
                },
                new ScheduleTask
                {
                    ScheduleTaskName = "User reminder - LastActivity",
                    Type = "ForeverNote.Services.Tasks.UserReminderLastActivityScheduleTask, ForeverNote.Services",
                    Enabled = false,
                    StopOnError = false,
                    TimeInterval = 1440
                },
                new ScheduleTask
                {
                    ScheduleTaskName = "User reminder - LastPurchase",
                    Type = "ForeverNote.Services.Tasks.UserReminderLastPurchaseScheduleTask, ForeverNote.Services",
                    Enabled = false,
                    StopOnError = false,
                    TimeInterval = 1440
                },
                new ScheduleTask
                {
                    ScheduleTaskName = "User reminder - Birthday",
                    Type = "ForeverNote.Services.Tasks.UserReminderBirthdayScheduleTask, ForeverNote.Services",
                    Enabled = false,
                    StopOnError = false,
                    TimeInterval = 1440
                },
                new ScheduleTask
                {
                    ScheduleTaskName = "User reminder - Completed order",
                    Type = "ForeverNote.Services.Tasks.UserReminderCompletedOrderScheduleTask, ForeverNote.Services",
                    Enabled = false,
                    StopOnError = false,
                    TimeInterval = 1440
                },
                new ScheduleTask
                {
                    ScheduleTaskName = "User reminder - Unpaid order",
                    Type = "ForeverNote.Services.Tasks.UserReminderUnpaidOrderScheduleTask, ForeverNote.Services",
                    Enabled = false,
                    StopOnError = false,
                    TimeInterval = 1440
                },
                new ScheduleTask
                {
                    ScheduleTaskName = "End of the auctions",
                    Type = "ForeverNote.Services.Tasks.EndAuctionsTask, ForeverNote.Services",
                    Enabled = false,
                    StopOnError = false,
                    TimeInterval = 60
                },
            };
            await _scheduleTaskRepository.InsertAsync(tasks);
        }

        protected virtual async Task InstallReturnRequestActions()
        {
            var returnRequestActions = new List<ReturnRequestAction>
                                {
                                    new ReturnRequestAction
                                        {
                                            Name = "Repair",
                                            DisplayOrder = 1
                                        },
                                    new ReturnRequestAction
                                        {
                                            Name = "Replacement",
                                            DisplayOrder = 2
                                        },
                                    new ReturnRequestAction
                                        {
                                            Name = "Store Credit",
                                            DisplayOrder = 3
                                        }
                                };
            await _returnRequestActionRepository.InsertAsync(returnRequestActions);
        }

        private async Task AddNoteTag(Note note, string tag)
        {
            var noteTag = _noteTagRepository.Table.FirstOrDefault(pt => pt.Name == tag);
            if (noteTag == null)
            {
                noteTag = new NoteTag {
                    Name = tag,
                };

                await _noteTagRepository.InsertAsync(noteTag);
            }
            noteTag.Count = noteTag.Count + 1;
            await _noteTagRepository.UpdateAsync(noteTag);
            note.NoteTags.Add(noteTag.Name);
            await _noteRepository.UpdateAsync(note);
        }

        private async Task CreateIndexes()
        {
            //version
            await _databaseVersionRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<ForeverNoteDatabaseVersion>((Builders<ForeverNoteDatabaseVersion>.IndexKeys.Ascending(x => x.Version)), new CreateIndexOptions() { Name = "DataBaseVersion", Unique = true }));

            //Language
            await _lsrRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<LocaleStringResource>((Builders<LocaleStringResource>.IndexKeys.Ascending(x => x.LanguageId).Ascending(x => x.ResourceName)), new CreateIndexOptions() { Name = "Language" }));
            await _lsrRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<LocaleStringResource>((Builders<LocaleStringResource>.IndexKeys.Ascending(x => x.ResourceName)), new CreateIndexOptions() { Name = "ResourceName" }));

            //user
            await _userRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<User>((Builders<User>.IndexKeys.Descending(x => x.CreatedOnUtc).Ascending(x => x.Deleted).Ascending("UserRoles._id")), new CreateIndexOptions() { Name = "CreatedOnUtc_1_UserRoles._id_1", Unique = false }));
            await _userRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<User>((Builders<User>.IndexKeys.Ascending(x => x.LastActivityDateUtc)), new CreateIndexOptions() { Name = "LastActivityDateUtc_1", Unique = false }));
            await _userRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<User>((Builders<User>.IndexKeys.Ascending(x => x.UserGuid)), new CreateIndexOptions() { Name = "UserGuid_1", Unique = false }));
            await _userRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<User>((Builders<User>.IndexKeys.Ascending(x => x.Email)), new CreateIndexOptions() { Name = "Email_1", Unique = false }));

            //user tag history
            await _userTagNoteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<UserTagNote>((Builders<UserTagNote>.IndexKeys.Ascending(x => x.Id).Ascending(x => x.DisplayOrder)), new CreateIndexOptions() { Name = "UserTagId_DisplayOrder", Unique = false }));

            //user history password
            await _userHistoryPasswordRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<UserHistoryPassword>((Builders<UserHistoryPassword>.IndexKeys.Ascending(x => x.UserId).Descending(x => x.CreatedOnUtc)), new CreateIndexOptions() { Name = "UserId", Unique = false }));

            //user api
            await _userapiRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<UserApi>((Builders<UserApi>.IndexKeys.Ascending(x => x.Email)), new CreateIndexOptions() { Name = "Email", Unique = true, Background = true }));

            //notebook
            await _notebookRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Notebook>(Builders<Notebook>.IndexKeys.Ascending(x => x.ShowOnHomePage).Ascending(x => x.DisplayOrder), new CreateIndexOptions() { Name = "ShowOnHomePage_DisplayOrder_1", Unique = false }));
            await _notebookRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Notebook>((Builders<Notebook>.IndexKeys.Ascending(x => x.ParentNotebookId).Ascending(x => x.DisplayOrder)), new CreateIndexOptions() { Name = "ParentNotebookId_1_DisplayOrder_1", Unique = false }));
            await _notebookRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Notebook>((Builders<Notebook>.IndexKeys.Ascending(x => x.DisplayOrder)), new CreateIndexOptions() { Name = "FeaturedNotesOnHomaPage_DisplayOrder_1", Unique = false }));

            //Note
            await _noteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Note>((Builders<Note>.IndexKeys.Ascending(x => x.MarkAsNew).Ascending(x => x.CreatedOnUtc)), new CreateIndexOptions() { Name = "MarkAsNew_1_CreatedOnUtc_1", Unique = false }));
            await _noteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Note>((Builders<Note>.IndexKeys.Ascending(x => x.ShowOnHomePage).Ascending(x => x.DisplayOrder).Ascending(x => x.Name)), new CreateIndexOptions() { Name = "ShowOnHomePage_1_Published_1", Unique = false }));
            await _noteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Note>((Builders<Note>.IndexKeys.Ascending(x => x.DisplayOrder)), new CreateIndexOptions() { Name = "ParentGroupedNoteId_1_DisplayOrder_1", Unique = false }));
            await _noteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Note>((Builders<Note>.IndexKeys.Ascending(x => x.NoteTags).Ascending(x => x.Name)), new CreateIndexOptions() { Name = "NoteTags._id_1_Name_1", Unique = false }));
            await _noteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Note>((Builders<Note>.IndexKeys.Ascending(x => x.Name)), new CreateIndexOptions() { Name = "Name_1", Unique = false }));

            await _noteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Note>((Builders<Note>.IndexKeys.Ascending("NoteNotebooks.NotebookId").Ascending("NoteNotebooks.DisplayOrder")), new CreateIndexOptions() { Name = "NoteNotebooks.NotebookId_1_DisplayOrder_1", Unique = false }));
            await _noteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Note>((Builders<Note>.IndexKeys.Ascending("NoteNotebooks.NotebookId").Ascending(x => x.DisplayOrderNotebook)), new CreateIndexOptions() { Name = "NoteNotebooks.NotebookId_1_OrderNotebook_1", Unique = false }));
            await _noteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Note>((Builders<Note>.IndexKeys.Ascending("NoteNotebooks.NotebookId").Ascending(x => x.Name)), new CreateIndexOptions() { Name = "NoteNotebooks.NotebookId_1_Name_1", Unique = false }));
            await _noteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Note>((Builders<Note>.IndexKeys.Ascending("NoteNotebooks.NotebookId")), new CreateIndexOptions() { Name = "NoteNotebooks.NotebookId_1_Sold_1", Unique = false }));
            await _noteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Note>((Builders<Note>.IndexKeys.Ascending("NoteNotebooks.NotebookId").Ascending("NoteNotebooks.IsFeaturedNote")), new CreateIndexOptions() { Name = "NoteNotebooks.NotebookId_1_IsFeaturedNote_1", Unique = false }));

            await _noteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Note>((Builders<Note>.IndexKeys.Ascending("NoteManufacturers.ManufacturerId")), new CreateIndexOptions() { Name = "NoteManufacturers.ManufacturerId_1_OrderNotebook_1", Unique = false }));
            await _noteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Note>((Builders<Note>.IndexKeys.Ascending("NoteManufacturers.ManufacturerId").Ascending(x => x.Name)), new CreateIndexOptions() { Name = "NoteManufacturers.ManufacturerId_1_Name_1", Unique = false }));
            await _noteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Note>((Builders<Note>.IndexKeys.Ascending("NoteManufacturers.ManufacturerId")), new CreateIndexOptions() { Name = "NoteManufacturers.ManufacturerId_1_Sold_1", Unique = false }));
            await _noteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Note>((Builders<Note>.IndexKeys.Ascending("NoteManufacturers.ManufacturerId").Ascending("NoteManufacturers.IsFeaturedNote")), new CreateIndexOptions() { Name = "NoteManufacturers.ManufacturerId_1_IsFeaturedNote_1", Unique = false }));

            await _noteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Note>((Builders<Note>.IndexKeys.Ascending("NoteSpecificationAttributes.SpecificationAttributeOptionId").Ascending("NoteSpecificationAttributes.AllowFiltering")), new CreateIndexOptions() { Name = "NoteSpecificationAttributes", Unique = false }));

            //Recently Viewed Notes
            await _recentlyViewedNoteRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<RecentlyViewedNote>((Builders<RecentlyViewedNote>.IndexKeys.Ascending(x => x.UserId).Ascending(x => x.NoteId).Descending(x => x.CreatedOnUtc)), new CreateIndexOptions() { Name = "UserId.NoteId" }));

            //message template
            await _messageTemplateRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<MessageTemplate>((Builders<MessageTemplate>.IndexKeys.Ascending(x => x.Name)), new CreateIndexOptions() { Name = "Name", Unique = false }));

            //Log
            await _logRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Log>((Builders<Log>.IndexKeys.Descending(x => x.CreatedOnUtc)), new CreateIndexOptions() { Name = "CreatedOnUtc", Unique = false }));

            //Campaign history
            await _campaignHistoryRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<CampaignHistory>((Builders<CampaignHistory>.IndexKeys.Ascending(x => x.CampaignId).Descending(x => x.CreatedDateUtc)), new CreateIndexOptions() { Name = "CampaignId", Unique = false }));

            //search term
            await _searchtermRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<SearchTerm>((Builders<SearchTerm>.IndexKeys.Descending(x => x.Count)), new CreateIndexOptions() { Name = "Count", Unique = false }));

            //setting
            await _settingRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<Setting>((Builders<Setting>.IndexKeys.Ascending(x => x.Name)), new CreateIndexOptions() { Name = "Name", Unique = false }));

            //externalauth
            await _externalAuthenticationRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<ExternalAuthenticationRecord>((Builders<ExternalAuthenticationRecord>.IndexKeys.Ascending(x => x.UserId)), new CreateIndexOptions() { Name = "UserId" }));

            //contactus
            await _contactUsRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<ContactUs>((Builders<ContactUs>.IndexKeys.Ascending(x => x.Email)), new CreateIndexOptions() { Name = "Email", Unique = false }));
            await _contactUsRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<ContactUs>((Builders<ContactUs>.IndexKeys.Descending(x => x.CreatedOnUtc)), new CreateIndexOptions() { Name = "CreatedOnUtc", Unique = false }));

            //user action
            await _userAction.Collection.Indexes.CreateOneAsync(new CreateIndexModel<UserAction>((Builders<UserAction>.IndexKeys.Ascending(x => x.ActionTypeId)), new CreateIndexOptions() { Name = "ActionTypeId", Unique = false }));

            await _userActionHistory.Collection.Indexes.CreateOneAsync(new CreateIndexModel<UserActionHistory>((Builders<UserActionHistory>.IndexKeys.Ascending(x => x.UserId).Ascending(x => x.UserActionId)), new CreateIndexOptions() { Name = "User_Action", Unique = false }));

            //banner
            await _popupArchive.Collection.Indexes.CreateOneAsync(new CreateIndexModel<PopupArchive>((Builders<PopupArchive>.IndexKeys.Ascending(x => x.UserActionId)), new CreateIndexOptions() { Name = "UserActionId", Unique = false }));

            //user reminder
            await _userReminderHistoryRepository.Collection.Indexes.CreateOneAsync(new CreateIndexModel<UserReminderHistory>((Builders<UserReminderHistory>.IndexKeys.Ascending(x => x.UserId).Ascending(x => x.UserReminderId)), new CreateIndexOptions() { Name = "UserId", Unique = false }));
        }

        private async Task CreateTables(string local)
        {
            if (string.IsNullOrEmpty(local))
                local = "en";

            try
            {
                var options = new CreateCollectionOptions();
                var collation = new Collation(local);
                options.Collation = collation;
                var dataSettingsManager = new DataSettingsManager();
                var connectionString = dataSettingsManager.LoadSettings().DataConnectionString;
                var mongoDBContext = new MongoDBContext(connectionString);
                var typeFinder = _serviceProvider.GetRequiredService<ITypeFinder>();
                var q = typeFinder.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "ForeverNote.Core");
                foreach (var item in q.GetTypes().Where(x => x.Namespace != null && x.Namespace.StartsWith("ForeverNote.Core.Domain")))
                {
                    if (item.BaseType != null && item.IsClass && item.BaseType == typeof(BaseEntity))
                        await mongoDBContext.Database().CreateCollectionAsync(item.Name, options);
                }
            }
            catch (Exception ex)
            {
                throw new ForeverNoteException(ex.Message);
            }
        }

        #endregion

        #region Methods


        public virtual async Task InstallData(string defaultUserEmail,
            string defaultUserPassword, string collation)
        {
            defaultUserEmail = defaultUserEmail.ToLower();
            await CreateTables(collation);
            await CreateIndexes();
            await InstallVersion();
            await InstallLanguages();
            await InstallUsersAndUsers(defaultUserEmail, defaultUserPassword);
            await InstallEmailAccounts();
            await InstallMessageTemplates();
            await InstallUserAction();
            await InstallSettings();
            await InstallLocaleResources();
            await InstallActivityLogTypes();
            await HashDefaultUserPassword(defaultUserEmail, defaultUserPassword);
            await InstallScheduleTasks();
            await InstallReturnRequestActions();
            await InstallNotebooks();
        }

        #endregion

    }
}
