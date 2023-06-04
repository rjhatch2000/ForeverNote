using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Templates;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Templates
{
    public class ProductTemplateValidator : BaseForeverNoteValidator<ProductTemplateModel>
    {
        public ProductTemplateValidator(
            IEnumerable<IValidatorConsumer<ProductTemplateModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.Templates.Product.Name.Required"));
            RuleFor(x => x.ViewPath).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.Templates.Product.ViewPath.Required"));
        }
    }
}