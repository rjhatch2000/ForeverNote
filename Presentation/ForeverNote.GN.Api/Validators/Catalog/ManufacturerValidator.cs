using FluentValidation;
using ForeverNote.Api.DTOs.Catalog;
using ForeverNote.Web.Framework.Extensions;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Media;
using System.Collections.Generic;

namespace ForeverNote.Api.Validators.Catalog
{
    public class ManufacturerValidator : BaseForeverNoteValidator<ManufacturerDto>
    {
        public ManufacturerValidator(IEnumerable<IValidatorConsumer<ManufacturerDto>> validators, 
            ILocalizationService localizationService, IPictureService pictureService, IManufacturerService manufacturerService, IManufacturerTemplateService manufacturerTemplateService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Api.Catalog.Manufacturer.Fields.Name.Required"));
            RuleFor(x => x.PageSizeOptions).Must(FluentValidationUtilities.PageSizeOptionsValidator).WithMessage(localizationService.GetResource("Api.Catalog.Manufacturer.Fields.PageSizeOptions.ShouldHaveUniqueItems"));
            RuleFor(x => x).MustAsync(async (x, y, context) =>
            {
                if (!string.IsNullOrEmpty(x.PictureId))
                {
                    var picture = await pictureService.GetPictureById(x.PictureId);
                    if (picture == null)
                        return false;
                }
                return true;
            }).WithMessage(localizationService.GetResource("Api.Catalog.Manufacturer.Fields.PictureId.NotExists"));

            RuleFor(x => x).MustAsync(async (x, y, context) =>
            {
                if (!string.IsNullOrEmpty(x.ManufacturerTemplateId))
                {
                    var template = await manufacturerTemplateService.GetManufacturerTemplateById(x.ManufacturerTemplateId);
                    if (template == null)
                        return false;
                }
                return true;
            }).WithMessage(localizationService.GetResource("Api.Catalog.Manufacturer.Fields.ManufacturerTemplateId.NotExists"));

            RuleFor(x => x).MustAsync(async (x, y, context) =>
            {
                if (!string.IsNullOrEmpty(x.Id))
                {
                    var manufacturer = await manufacturerService.GetManufacturerById(x.Id);
                    if (manufacturer == null)
                        return false;
                }
                return true;
            }).WithMessage(localizationService.GetResource("Api.Catalog.Manufacturer.Fields.Id.NotExists"));
        }
    }
}
