﻿using ForeverNote.Core.Domain.Customers;
using ForeverNote.Web.Areas.Admin.Models.Settings;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class RewardPointsSettingsMappingExtensions
    {
        public static RewardPointsSettingsModel ToModel(this RewardPointsSettings entity)
        {
            return entity.MapTo<RewardPointsSettings, RewardPointsSettingsModel>();
        }
        public static RewardPointsSettings ToEntity(this RewardPointsSettingsModel model, RewardPointsSettings destination)
        {
            return model.MapTo(destination);
        }
    }
}