using FluentValidation;
using ForeverNote.Web.Framework.Extensions;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Catalog;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Catalog
{
    public class ManufacturerValidator : BaseForeverNoteValidator<ManufacturerModel>
    {
        public ManufacturerValidator(
            IEnumerable<IValidatorConsumer<ManufacturerModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Catalog.Manufacturers.Fields.Name.Required"));
            RuleFor(x => x.PageSizeOptions).Must(FluentValidationUtilities.PageSizeOptionsValidator).WithMessage(localizationService.GetResource("Admin.Catalog.Manufacturers.Fields.PageSizeOptions.ShouldHaveUniqueItems"));
        }
    }
} 
