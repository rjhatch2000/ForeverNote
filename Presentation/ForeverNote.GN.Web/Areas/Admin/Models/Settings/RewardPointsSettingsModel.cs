﻿using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Settings
{
    [Validator(typeof(RewardPointsSettingsValidator))]
    public partial class RewardPointsSettingsModel : BaseForeverNoteModel
    {
        public RewardPointsSettingsModel()
        {
            PointsForPurchases_Awarded_OrderStatuses = new List<SelectListItem>();
        }
        public string ActiveStoreScopeConfiguration { get; set; }


        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.RewardPoints.Enabled")]
        public bool Enabled { get; set; }
        public bool Enabled_OverrideForStore { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.RewardPoints.ExchangeRate")]
        public decimal ExchangeRate { get; set; }
        public bool ExchangeRate_OverrideForStore { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.RewardPoints.MinimumRewardPointsToUse")]
        public int MinimumRewardPointsToUse { get; set; }
        public bool MinimumRewardPointsToUse_OverrideForStore { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.RewardPoints.PointsForRegistration")]
        public int PointsForRegistration { get; set; }
        public bool PointsForRegistration_OverrideForStore { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.RewardPoints.PointsForPurchases_Amount")]
        public decimal PointsForPurchases_Amount { get; set; }
        public int PointsForPurchases_Points { get; set; }
        public bool PointsForPurchases_OverrideForStore { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.RewardPoints.PointsForPurchases_Awarded")]
        public int PointsForPurchases_Awarded { get; set; }
        public bool PointsForPurchases_Awarded_OverrideForStore { get; set; }
        public IList<SelectListItem> PointsForPurchases_Awarded_OrderStatuses { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.RewardPoints.ReduceRewardPointsAfterCancelOrder")]
        public bool ReduceRewardPointsAfterCancelOrder { get; set; }
        public bool ReduceRewardPointsAfterCancelOrder_OverrideForStore { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.RewardPoints.DisplayHowMuchWillBeEarned")]
        public bool DisplayHowMuchWillBeEarned { get; set; }
        public bool DisplayHowMuchWillBeEarned_OverrideForStore { get; set; }

        public string PrimaryStoreCurrencyCode { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.RewardPoints.PointsAccumulatedForAllStores")]
        public bool PointsAccumulatedForAllStores { get; set; }


    }
}