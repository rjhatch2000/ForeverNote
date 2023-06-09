﻿using System;

namespace ForeverNote.Core.Domain.Messages
{
    /// <summary>
    /// Represents a campaign history
    /// </summary>
    public partial class CampaignHistory : BaseEntity
    {
        
        public string CampaignId { get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedDateUtc { get; set; }

    }
}
