using ForeverNote.Core;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Services.Users;
using ForeverNote.Services.Events;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Logging;
using ForeverNote.Services.Messages.DotLiquidDrops;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Messages
{
    public partial class CampaignService : ICampaignService
    {
        private readonly IRepository<Campaign> _campaignRepository;
        private readonly IRepository<CampaignHistory> _campaignHistoryRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IUserService _userService;
        private readonly IMediator _mediator;
        private readonly IUserActivityService _userActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;

        public CampaignService(
            IRepository<Campaign> campaignRepository,
            IRepository<CampaignHistory> campaignHistoryRepository,
            IRepository<User> userRepository,
            IEmailSender emailSender, IMessageTokenProvider messageTokenProvider,
            IQueuedEmailService queuedEmailService,
            IUserService userService,
            IMediator mediator,
            IUserActivityService userActivityService,
            ILocalizationService localizationService,
            ILanguageService languageService
        )
        {
            _campaignRepository = campaignRepository;
            _campaignHistoryRepository = campaignHistoryRepository;
            _userRepository = userRepository;
            _emailSender = emailSender;
            _messageTokenProvider = messageTokenProvider;
            _queuedEmailService = queuedEmailService;
            _userService = userService;
            _mediator = mediator;
            _userActivityService = userActivityService;
            _localizationService = localizationService;
            _languageService = languageService;
        }

        /// <summary>
        /// Inserts a campaign
        /// </summary>
        /// <param name="campaign">Campaign</param>        
        public virtual async Task InsertCampaign(Campaign campaign)
        {
            if (campaign == null)
                throw new ArgumentNullException("campaign");

            await _campaignRepository.InsertAsync(campaign);

            //event notification
            await _mediator.EntityInserted(campaign);
        }

        /// <summary>
        /// Inserts a campaign history
        /// </summary>
        /// <param name="campaign">Campaign</param>        
        public virtual async Task InsertCampaignHistory(CampaignHistory campaignhistory)
        {
            if (campaignhistory == null)
                throw new ArgumentNullException("campaignhistory");

            await _campaignHistoryRepository.InsertAsync(campaignhistory);

        }

        /// <summary>
        /// Updates a campaign
        /// </summary>
        /// <param name="campaign">Campaign</param>
        public virtual async Task UpdateCampaign(Campaign campaign)
        {
            if (campaign == null)
                throw new ArgumentNullException("campaign");

            await _campaignRepository.UpdateAsync(campaign);

            //event notification
            await _mediator.EntityUpdated(campaign);
        }

        /// <summary>
        /// Deleted a queued email
        /// </summary>
        /// <param name="campaign">Campaign</param>
        public virtual async Task DeleteCampaign(Campaign campaign)
        {
            if (campaign == null)
                throw new ArgumentNullException("campaign");

            await _campaignRepository.DeleteAsync(campaign);

            //event notification
            await _mediator.EntityDeleted(campaign);
        }

        /// <summary>
        /// Gets a campaign by identifier
        /// </summary>
        /// <param name="campaignId">Campaign identifier</param>
        /// <returns>Campaign</returns>
        public virtual Task<Campaign> GetCampaignById(string campaignId)
        {
            return _campaignRepository.GetByIdAsync(campaignId);

        }

        /// <summary>
        /// Gets all campaigns
        /// </summary>
        /// <returns>Campaigns</returns>
        public virtual async Task<IList<Campaign>> GetAllCampaigns()
        {

            var query = from c in _campaignRepository.Table
                        orderby c.CreatedOnUtc
                        select c;
            return await query.ToListAsync();
        }

        public virtual async Task<IPagedList<CampaignHistory>> GetCampaignHistory(Campaign campaign, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            if (campaign == null)
                throw new ArgumentNullException("campaign");

            var query = from c in _campaignHistoryRepository.Table
                        where c.CampaignId == campaign.Id
                        orderby c.CreatedDateUtc descending
                        select c;
            return await PagedList<CampaignHistory>.Create(query, pageIndex, pageSize);
        }

        private class CampaignUserHelp
        {
            public CampaignUserHelp()
            {
            }
            public string UserId { get; set; }
            public string UserEmail { get; set; }
            public string Email { get; set; }
            public DateTime CreatedOnUtc { get; set; }
            public DateTime LastActivityDateUtc { get; set; }
            public ICollection<string> UserTags { get; set; }
        }

        /// <summary>
        /// Sends a campaign to specified email
        /// </summary>
        /// <param name="campaign">Campaign</param>
        /// <param name="emailAccount">Email account</param>
        /// <param name="email">Email</param>
        public virtual async Task SendCampaign(Campaign campaign, EmailAccount emailAccount, string email)
        {
            if (campaign == null)
                throw new ArgumentNullException("campaign");

            if (emailAccount == null)
                throw new ArgumentNullException("emailAccount");

            var language = await _languageService.GetLanguageById(campaign.LanguageId);
            if (language == null)
                language = (await _languageService.GetAllLanguages()).FirstOrDefault();

            var liquidObject = new LiquidObject();
            var user = await _userService.GetUserByEmail(email);
            if (user != null)
            {
                await _messageTokenProvider.AddUserTokens(liquidObject, user, language);
            }

            var body = LiquidExtensions.Render(liquidObject, campaign.Body);
            var subject = LiquidExtensions.Render(liquidObject, campaign.Subject);

            await _emailSender.SendEmail(emailAccount, subject, body, emailAccount.Email, emailAccount.DisplayName, email, null);
        }
    }
}