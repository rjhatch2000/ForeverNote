using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Templates;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Templates
{
    public class ManufacturerTemplateValidator : BaseForeverNoteValidator<ManufacturerTemplateModel>
    {
        public ManufacturerTemplateValidator(
            IEnumerable<IValidatorConsumer<ManufacturerTemplateModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.Templates.Manufacturer.Name.Required"));
            RuleFor(x => x.ViewPath).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.Templates.Manufacturer.ViewPath.Required"));
        }
    }
}