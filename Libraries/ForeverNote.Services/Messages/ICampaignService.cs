﻿using ForeverNote.Core;
using ForeverNote.Core.Domain.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Messages
{
    public partial interface ICampaignService
    {
        /// <summary>
        /// Inserts a campaign
        /// </summary>
        /// <param name="campaign">Campaign</param>        
        Task InsertCampaign(Campaign campaign);

        /// <summary>
        /// Inserts a campaign history
        /// </summary>
        /// <param name="campaign">Campaign</param>        
        Task InsertCampaignHistory(CampaignHistory campaignhistory);
        /// <summary>
        /// Updates a campaign
        /// </summary>
        /// <param name="campaign">Campaign</param>
        Task UpdateCampaign(Campaign campaign);

        /// <summary>
        /// Deleted a queued email
        /// </summary>
        /// <param name="campaign">Campaign</param>
        Task DeleteCampaign(Campaign campaign);

        /// <summary>
        /// Gets a campaign by identifier
        /// </summary>
        /// <param name="campaignId">Campaign identifier</param>
        /// <returns>Campaign</returns>
        Task<Campaign> GetCampaignById(string campaignId);

        /// <summary>
        /// Gets all campaigns
        /// </summary>
        /// <returns>Campaigns</returns>
        Task<IList<Campaign>> GetAllCampaigns();

        /// <summary>
        /// Gets campaign history 
        /// </summary>
        /// <returns>Campaigns</returns>
        Task<IPagedList<CampaignHistory>> GetCampaignHistory(Campaign campaign, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Sends a campaign to specified email
        /// </summary>
        /// <param name="campaign">Campaign</param>
        /// <param name="emailAccount">Email account</param>
        /// <param name="email">Email</param>
        Task SendCampaign(Campaign campaign, EmailAccount emailAccount, string email);
    }
}
