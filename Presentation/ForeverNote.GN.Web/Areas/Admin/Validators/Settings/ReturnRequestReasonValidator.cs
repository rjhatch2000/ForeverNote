using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Settings;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Settings
{
    public class ReturnRequestReasonValidator : BaseForeverNoteValidator<ReturnRequestReasonModel>
    {
        public ReturnRequestReasonValidator(
            IEnumerable<IValidatorConsumer<ReturnRequestReasonModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Settings.Order.ReturnRequestReasons.Name.Required"));
        }
    }
}