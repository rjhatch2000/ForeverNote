using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Settings;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Settings
{
    public class ReturnRequestActionValidator : BaseForeverNoteValidator<ReturnRequestActionModel>
    {
        public ReturnRequestActionValidator(
            IEnumerable<IValidatorConsumer<ReturnRequestActionModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Settings.Order.ReturnRequestActions.Name.Required"));
        }
    }
}