using FluentValidation;
using ForeverNote.Api.DTOs.Catalog;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Localization;
using System.Collections.Generic;

namespace ForeverNote.Api.Validators.Catalog
{
    public class ProductAttributeValidator : BaseForeverNoteValidator<ProductAttributeDto>
    {
        public ProductAttributeValidator(IEnumerable<IValidatorConsumer<ProductAttributeDto>> validators,
            ILocalizationService localizationService, IProductAttributeService productAttributeService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Api.Catalog.ProductAttribute.Fields.Name.Required"));
            RuleFor(x => x).MustAsync(async (x, y, context) =>
            {
                if (!string.IsNullOrEmpty(x.Id))
                {
                    var pa = await productAttributeService.GetProductAttributeById(x.Id);
                    if (pa == null)
                        return false;
                }
                return true;
            }).WithMessage(localizationService.GetResource("Api.Catalog.ProductAttribute.Fields.Id.NotExists"));
            RuleFor(x => x).Must((x, context) =>
            {
                foreach (var item in x.PredefinedProductAttributeValues)
                {
                    if (string.IsNullOrEmpty(item.Name))
                        return false;
                }
                return true;
            }).WithMessage(localizationService.GetResource("Api.Catalog.PredefinedProductAttributeValue.Fields.Name.Required"));
        }
    }
}
