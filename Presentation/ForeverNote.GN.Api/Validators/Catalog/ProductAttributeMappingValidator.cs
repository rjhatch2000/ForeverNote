using FluentValidation;
using ForeverNote.Api.DTOs.Catalog;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Localization;
using System.Collections.Generic;

namespace ForeverNote.Api.Validators.Catalog
{
    public class ProductAttributeMappingValidator : BaseForeverNoteValidator<ProductAttributeMappingDto>
    {
        public ProductAttributeMappingValidator(
            IEnumerable<IValidatorConsumer<ProductAttributeMappingDto>> validators,
            ILocalizationService localizationService, IProductAttributeService productAttributeService)
            : base(validators)
        {
            RuleFor(x => x).MustAsync(async (x, y, context) =>
            {
                var productattribute = await productAttributeService.GetProductAttributeById(x.ProductAttributeId);
                if (productattribute == null)
                    return false;
                return true;
            }).WithMessage(localizationService.GetResource("Api.Catalog.ProductAttributeMapping.Fields.ProductAttributeId.NotExists"));
        }
    }
}
