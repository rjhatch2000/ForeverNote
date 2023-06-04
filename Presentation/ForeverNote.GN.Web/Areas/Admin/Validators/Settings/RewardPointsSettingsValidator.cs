using FluentValidation;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Settings;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Settings
{
    public class RewardPointsSettingsValidator : BaseForeverNoteValidator<RewardPointsSettingsModel>
    {
        public RewardPointsSettingsValidator(
            IEnumerable<IValidatorConsumer<RewardPointsSettingsModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.PointsForPurchases_Awarded).NotEqual((int)OrderStatus.Pending).WithMessage(localizationService.GetResource("Admin.Configuration.Settings.RewardPoints.PointsForPurchases_Awarded.Pending"));
        }
    }
}