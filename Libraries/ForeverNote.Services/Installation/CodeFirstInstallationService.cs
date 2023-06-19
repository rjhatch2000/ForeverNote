using ForeverNote.Core;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Configuration;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Logging;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Core.Domain.Notebooks;
using ForeverNote.Core.Domain.Notes;
using ForeverNote.Core.Domain.Tasks;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Infrastructure.TypeSearch;
using ForeverNote.Services.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Installation
{
    public partial class InstallationService : IInstallationService
    {
        #region Fields

        private readonly IRepository<ForeverNoteVersion> _versionRepository;
        private readonly IRepository<CampaignHistory> _campaignHistoryRepository;
        private readonly IRepository<Campaign> _campaignRepository;
        private readonly IRepository<Language> _languageRepository;
        private readonly IRepository<Log> _logRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserTagNote> _userTagNoteRepository;
        private readonly IRepository<UserHistoryPassword> _userHistoryPasswordRepository;
        private readonly IRepository<UserApi> _userapiRepository;
        private readonly IRepository<UserAttribute> _userAttributeRepository;
        private readonly IRepository<Notebook> _notebookRepository;
        private readonly IRepository<Note> _noteRepository;
        private readonly IRepository<EmailAccount> _emailAccountRepository;
        private readonly IRepository<MessageTemplate> _messageTemplateRepository;
        private readonly IRepository<ActivityLog> _activityLogRepository;
        private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
        private readonly IRepository<NoteTag> _noteTagRepository;
        private readonly IRepository<ScheduleTask> _scheduleTaskRepository;
        private readonly IRepository<SearchTerm> _searchtermRepository;
        private readonly IRepository<Setting> _settingRepository;
        private readonly IRepository<ContactUs> _contactUsRepository;
        private readonly IRepository<RecentlyViewedNote> _recentlyViewedNoteRepository;
        private readonly IRepository<QueuedEmail> _queuedEmailRepository;


        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Ctor

        public InstallationService(
            IServiceProvider serviceProvider,
            IWebHostEnvironment webHostEnvironment,
            IRepository<ForeverNoteVersion> versionRepository,
            IRepository<CampaignHistory> campaignHistoryRepository,
            IRepository<Campaign> campaignRepository,
            IRepository<Language> languageRepository,
            IRepository<Log> logRepository,
            IRepository<User> userRepository,
            IRepository<UserTagNote> userTagNoteRepository,
            IRepository<UserHistoryPassword> userHistoryPasswordRepository,
            IRepository<UserApi> userapiRepository,
            IRepository<UserAttribute> userAttributeRepository,
            IRepository<Notebook> notebookRepository,
            IRepository<Note> noteRepository,
            IRepository<EmailAccount> emailAccountRepository,
            IRepository<MessageTemplate> messageTemplateRepository,
            IRepository<ActivityLog> activityLogRepository,
            IRepository<ActivityLogType> activityLogTypeRepository,
            IRepository<NoteTag> noteTagRepository,
            IRepository<ScheduleTask> scheduleTaskRepository,
            IRepository<SearchTerm> searchtermRepository,
            IRepository<Setting> settingRepository,
            IRepository<ContactUs> contactUsRepository,
            IRepository<RecentlyViewedNote> recentlyViewedNoteRepository,
            IRepository<QueuedEmail> queuedEmailRepository
        )
        {

            _versionRepository = versionRepository;
            _campaignHistoryRepository = campaignHistoryRepository;
            _campaignRepository = campaignRepository;
            _languageRepository = languageRepository;
            _logRepository = logRepository;
            _userRepository = userRepository;
            _userTagNoteRepository = userTagNoteRepository;
            _userHistoryPasswordRepository = userHistoryPasswordRepository;
            _userapiRepository = userapiRepository;
            _userAttributeRepository = userAttributeRepository;
            _notebookRepository = notebookRepository;
            _noteRepository = noteRepository;
            _emailAccountRepository = emailAccountRepository;
            _messageTemplateRepository = messageTemplateRepository;
            _activityLogTypeRepository = activityLogTypeRepository;
            _noteTagRepository = noteTagRepository;
            _recentlyViewedNoteRepository = recentlyViewedNoteRepository;
            _scheduleTaskRepository = scheduleTaskRepository;
            _searchtermRepository = searchtermRepository;
            _settingRepository = settingRepository;
            _contactUsRepository = contactUsRepository;
            _queuedEmailRepository = queuedEmailRepository;
            _activityLogRepository = activityLogRepository;
            _hostingEnvironment = webHostEnvironment;
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Utilities

        protected virtual string GetSamplesPath()
        {
            return Path.Combine(_hostingEnvironment.WebRootPath, "assets/samples/");
        }


        protected virtual async Task InstallVersion()
        {
            var version = new ForeverNoteVersion
            {
                DataBaseVersion = ForeverNote.Core.Infrastructure.ForeverNoteVersion.SupportedDBVersion
            };
            await _versionRepository.InsertAsync(version);
        }

        protected virtual async Task HashDefaultUserPassword(string defaultUserEmail, string defaultUserPassword)
        {
            var userManagerService = _serviceProvider.GetRequiredService<IUserManagerService>();
            await userManagerService.ChangePassword(new ChangePasswordRequest(defaultUserEmail, PasswordFormat.Hashed, defaultUserPassword));
        }

        private async Task CreateIndexes(IDatabaseContext dbContext, DataSettings dataSettings)
        {
            //version
            await dbContext.CreateIndex(_versionRepository, OrderBuilder<ForeverNoteVersion>.Create().Ascending(x => x.DataBaseVersion), "DataBaseVersion", true);

            //////Language
            ////await dbContext.CreateIndex(_lsrRepository, OrderBuilder<TranslationResource>.Create().Ascending(x => x.LanguageId).Ascending(x => x.Name), "Language");
            ////await dbContext.CreateIndex(_lsrRepository, OrderBuilder<TranslationResource>.Create().Ascending(x => x.Name), "ResourceName");
            ////await dbContext.CreateIndex(_languageRepository, OrderBuilder<Language>.Create().Ascending(x => x.DisplayOrder), "DisplayOrder");

            //user
            await dbContext.CreateIndex(_userRepository, OrderBuilder<User>.Create().Descending(x => x.CreatedOnUtc).Ascending(x => x.Deleted), "CreatedOnUtc_1");
            await dbContext.CreateIndex(_userRepository, OrderBuilder<User>.Create().Ascending(x => x.LastActivityDateUtc), "LastActivityDateUtc_1");
            await dbContext.CreateIndex(_userRepository, OrderBuilder<User>.Create().Ascending(x => x.UserGuid), "UserGuid_1");
            await dbContext.CreateIndex(_userRepository, OrderBuilder<User>.Create().Ascending(x => x.Email), "Email_1");
            await dbContext.CreateIndex(_userRepository, OrderBuilder<User>.Create().Descending(x => x.CreatedOnUtc), "CreatedOnUtc");

            await dbContext.CreateIndex(_userAttributeRepository, OrderBuilder<UserAttribute>.Create().Ascending(x => x.DisplayOrder), "DisplayOrder");

            await dbContext.CreateIndex(_userHistoryPasswordRepository, OrderBuilder<UserHistoryPassword>.Create().Ascending(x => x.CreatedOnUtc), "CreatedOnUtc");

            //user tag history
            await dbContext.CreateIndex(_userTagNoteRepository, OrderBuilder<UserTagNote>.Create().Ascending(x => x.Id).Ascending(x => x.DisplayOrder), "UserTagId_DisplayOrder");
            await dbContext.CreateIndex(_userTagNoteRepository, OrderBuilder<UserTagNote>.Create().Ascending(x => x.DisplayOrder), "DisplayOrder");

            //user history password
            await dbContext.CreateIndex(_userHistoryPasswordRepository, OrderBuilder<UserHistoryPassword>.Create().Ascending(x => x.UserId).Descending(x => x.CreatedOnUtc), "UserId");

            //user api
            await dbContext.CreateIndex(_userapiRepository, OrderBuilder<UserApi>.Create().Ascending(x => x.Email), "Email", true);

            //notebook
            await dbContext.CreateIndex(_notebookRepository, OrderBuilder<Notebook>.Create().Ascending(x => x.ShowOnHomePage).Ascending(x => x.DisplayOrder), "ShowOnHomePage_DisplayOrder_1");
            await dbContext.CreateIndex(_notebookRepository, OrderBuilder<Notebook>.Create().Ascending(x => x.ParentNotebookId).Ascending(x => x.DisplayOrder), "ParentNotebookId_1_DisplayOrder_1");
            await dbContext.CreateIndex(_notebookRepository, OrderBuilder<Notebook>.Create().Ascending(x => x.FeaturedNotesOnHomePage).Ascending(x => x.DisplayOrder), "FeaturedNotesOnHomePage_DisplayOrder_1");
            await dbContext.CreateIndex(_notebookRepository, OrderBuilder<Notebook>.Create().Ascending(x => x.SearchBoxDisplayOrder), "SearchBoxDisplayOrder");
            await dbContext.CreateIndex(_notebookRepository, OrderBuilder<Notebook>.Create().Ascending(x => x.ParentNotebookId).Ascending(x => x.DisplayOrder).Ascending(x => x.Name), "ParentNotebookId_1_DisplayOrder_1_Name_1");

            await dbContext.CreateIndex(_notebookRepository, OrderBuilder<Notebook>.Create().Ascending(x => x.DisplayOrder), "DisplayOrder_1");

            //Note
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending(x => x.MarkAsNew).Ascending(x => x.CreatedOnUtc), "MarkAsNew_1_CreatedOnUtc_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending(x => x.ShowOnHomePage).Ascending(x => x.DisplayOrder).Ascending(x => x.Name), "ShowOnHomePage_1_Published_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending(x => x.DisplayOrder).Ascending(x => x.Name), "ShowOnBestSeller_1_Published_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending(x => x.DisplayOrder), "ParentGroupedNoteId_1_DisplayOrder_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending(x => x.NoteTags).Ascending(x => x.Name), "NoteTags._id_1_Name_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending(x => x.Name), "Name_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending("NoteNotebooks.DisplayOrder"), "NotebookId_1_DisplayOrder_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending(x => x.DisplayOrderNotebook).Ascending("NoteNotebooks.NotebookId"), "NoteNotebooks.NotebookId_1_OrderNotebook_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending(x => x.Name).Ascending("NoteNotebooks.NotebookId"), "NoteNotebooks.NotebookId_1_Name_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending("NoteNotebooks.NotebookId"), "NoteNotebooks.NotebookId_1_Price_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending("NoteNotebooks.NotebookId"), "NoteNotebooks.NotebookId_1_Sold_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending("NoteNotebooks.NotebookId").Ascending("NoteNotebooks.IsFeaturedNote"), "NoteNotebooks.NotebookId_1_IsFeaturedNote_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending("NoteCollections.CollectionId"), "NoteCollections.CollectionId_1_OrderNotebook_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending("NoteCollections.CollectionId").Ascending(x => x.Name), "NoteCollections.CollectionId_1_Name_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending("NoteCollections.CollectionId"), "NoteCollections.CollectionId_1_Sold_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending("NoteCollections.CollectionId").Ascending("NoteCollections.IsFeaturedNote"), "NoteCollections.CollectionId_1_IsFeaturedNote_1");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending("NoteSpecificationAttributes.SpecificationAttributeOptionId").Ascending("NoteSpecificationAttributes.AllowFiltering"), "NoteSpecificationAttributes");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending(x => x.DisplayOrder).Ascending(x => x.Name), "DisplayOrder_Name");

            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending(x => x.DisplayOrderNotebook), "DisplayOrderNotebook");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending(x => x.DisplayOrder), "DisplayOrder");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending(x => x.CreatedOnUtc), "CreatedOnUtc");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending(x => x.OnSale), "OnSale");
            await dbContext.CreateIndex(_noteRepository, OrderBuilder<Note>.Create().Ascending(x => x.Viewed), "Viewed");

            //Recently Viewed Notes
            await dbContext.CreateIndex(_recentlyViewedNoteRepository, OrderBuilder<RecentlyViewedNote>.Create().Ascending(x => x.UserId).Ascending(x => x.NoteId).Descending(x => x.CreatedOnUtc), "UserId.NoteId");
            await dbContext.CreateIndex(_recentlyViewedNoteRepository, OrderBuilder<RecentlyViewedNote>.Create().Descending(x => x.CreatedOnUtc), "CreatedOnUtc");

            //message template
            await dbContext.CreateIndex(_messageTemplateRepository, OrderBuilder<MessageTemplate>.Create().Ascending(x => x.Name), "Name");

            //Log
            await dbContext.CreateIndex(_logRepository, OrderBuilder<Log>.Create().Ascending(x => x.CreatedOnUtc), "CreatedOnUtc");

            //Campaign 
            await dbContext.CreateIndex(_campaignRepository, OrderBuilder<Campaign>.Create().Ascending(x => x.CreatedOnUtc), "CreatedOnUtc");
            await dbContext.CreateIndex(_campaignHistoryRepository, OrderBuilder<CampaignHistory>.Create().Ascending(x => x.CampaignId).Descending(x => x.CreatedDateUtc), "CampaignId");

            //search term
            await dbContext.CreateIndex(_searchtermRepository, OrderBuilder<SearchTerm>.Create().Descending(x => x.Count), "Count");

            //setting
            await dbContext.CreateIndex(_settingRepository, OrderBuilder<Setting>.Create().Ascending(x => x.Name), "Name");

            //queuemail
            await dbContext.CreateIndex(_queuedEmailRepository, OrderBuilder<QueuedEmail>.Create().Descending(x => x.CreatedOnUtc), "CreatedOnUtc");
            await dbContext.CreateIndex(_queuedEmailRepository, OrderBuilder<QueuedEmail>.Create().Descending(x => x.PriorityId).Ascending(x => x.CreatedOnUtc), "PriorityId_CreatedOnUtc");

            //contactus
            await dbContext.CreateIndex(_contactUsRepository, OrderBuilder<ContactUs>.Create().Ascending(x => x.Email), "Email");
            await dbContext.CreateIndex(_contactUsRepository, OrderBuilder<ContactUs>.Create().Descending(x => x.CreatedOnUtc), "CreatedOnUtc");

            //useractivity 
            await dbContext.CreateIndex(_activityLogTypeRepository, OrderBuilder<ActivityLogType>.Create().Ascending(x => x.Name), "Name");
            await dbContext.CreateIndex(_activityLogRepository, OrderBuilder<ActivityLog>.Create().Ascending(x => x.CreatedOnUtc), "CreatedOnUtc");

            //if(dataSettings.DbProvider == DbProvider.CosmosDB)
            //{
            //    //
            //    //db.fs.chunks.createIndex({'n': 1})
            //    //To Fix problem with download files from GridFSBucket
            //    //
            //}

        }

        private async Task CreateTables(string local)
        {
            try
            {
                var dataSettings = DataSettingsManager.LoadSettings(reloadSettings: true);
                var dbContext = _serviceProvider.GetRequiredService<IDatabaseContext>();
                dbContext.SetConnection(dataSettings.ConnectionString);

                if (dbContext.InstallProcessCreateTable)
                {
                    var typeSearcher = _serviceProvider.GetRequiredService<ITypeSearcher>();
                    var q = typeSearcher.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "ForeverNote.Domain");

                    foreach (var item in q.GetTypes())
                    {
                        if (item.BaseType != null && item.IsClass && item.BaseType == typeof(BaseEntity))
                            await dbContext.CreateTable(item.Name, local);
                    }
                }

                if (dbContext.InstallProcessCreateIndex)
                    await CreateIndexes(dbContext, dataSettings);

            }
            catch (Exception ex)
            {
                throw new ForeverNoteException(ex.Message);
            }
        }

        #endregion

        #region Methods


        public virtual async Task InstallData(string httpscheme, HostString host, string defaultUserEmail,
            string defaultUserPassword, string collation, bool installSampleData = true, string companyName = "", string companyAddress = "",
            string companyPhoneNumber = "", string companyEmail = "")
        {
            defaultUserEmail = defaultUserEmail.ToLower();

            await CreateTables(collation);
            await InstallVersion();
            await InstallLanguages();
            await InstallUsers(defaultUserEmail, defaultUserPassword);
            await InstallEmailAccounts();
            await InstallMessageTemplates();
            await InstallSettings(installSampleData);
            await InstallLocaleResources();
            await InstallActivityLogTypes();
            await InstallScheduleTasks();
            await InstallNotebooks();
            await InstallNotes(defaultUserEmail);
        }


        #endregion

    }
}
