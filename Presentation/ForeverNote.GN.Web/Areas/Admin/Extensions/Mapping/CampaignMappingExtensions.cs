﻿using ForeverNote.Core.Domain.Messages;
using ForeverNote.Web.Areas.Admin.Models.Messages;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class CampaignMappingExtensions
    {
        public static CampaignModel ToModel(this Campaign entity)
        {
            return entity.MapTo<Campaign, CampaignModel>();
        }

        public static Campaign ToEntity(this CampaignModel model)
        {
            return model.MapTo<CampaignModel, Campaign>();
        }

        public static Campaign ToEntity(this CampaignModel model, Campaign destination)
        {
            return model.MapTo(destination);
        }
    }
}