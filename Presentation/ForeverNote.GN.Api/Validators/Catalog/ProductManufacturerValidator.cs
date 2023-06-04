using FluentValidation;
using ForeverNote.Api.DTOs.Catalog;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Localization;
using System.Collections.Generic;

namespace ForeverNote.Api.Validators.Catalog
{
    public class ProductManufacturerValidator : BaseForeverNoteValidator<ProductManufacturerDto>
    {
        public ProductManufacturerValidator(IEnumerable<IValidatorConsumer<ProductManufacturerDto>> validators, ILocalizationService localizationService, IManufacturerService manufacturerService)
            : base(validators)
        {
            RuleFor(x => x).MustAsync(async (x, y, context) =>
            {
                var manufacturer = await manufacturerService.GetManufacturerById(x.ManufacturerId);
                if (manufacturer == null)
                    return false;
                return true;
            }).WithMessage(localizationService.GetResource("Api.Catalog.ProductManufacturer.Fields.ManufacturerId.NotExists"));
        }
    }
}
