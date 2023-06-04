using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Shipping;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Shipping
{
    public class ShippingMethodValidator : BaseForeverNoteValidator<ShippingMethodModel>
    {
        public ShippingMethodValidator(
            IEnumerable<IValidatorConsumer<ShippingMethodModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Shipping.Methods.Fields.Name.Required"));
        }
    }
}