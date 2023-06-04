using FluentValidation;
using ForeverNote.Web.Framework.Extensions;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Catalog;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Catalog
{
    public class CategoryValidator : BaseForeverNoteValidator<CategoryModel>
    {
        public CategoryValidator(
            IEnumerable<IValidatorConsumer<CategoryModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Catalog.Categories.Fields.Name.Required"));
            RuleFor(x => x.PageSizeOptions).Must(FluentValidationUtilities.PageSizeOptionsValidator).WithMessage(localizationService.GetResource("Admin.Catalog.Categories.Fields.PageSizeOptions.ShouldHaveUniqueItems"));
        }
    }
}