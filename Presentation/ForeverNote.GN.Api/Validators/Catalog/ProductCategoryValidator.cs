using FluentValidation;
using ForeverNote.Api.DTOs.Catalog;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Localization;
using System.Collections.Generic;

namespace ForeverNote.Api.Validators.Catalog
{
    public class ProductCategoryValidator : BaseForeverNoteValidator<ProductCategoryDto>
    {
        public ProductCategoryValidator(IEnumerable<IValidatorConsumer<ProductCategoryDto>> validators, ILocalizationService localizationService, ICategoryService categoryService)
            : base(validators)
        {
            RuleFor(x => x).MustAsync(async (x, y, context) =>
            {
                var category = await categoryService.GetCategoryById(x.CategoryId);
                if (category == null)
                    return false;
                return true;
            }).WithMessage(localizationService.GetResource("Api.Catalog.ProductCategory.Fields.CategoryId.NotExists"));
        }
    }
}
