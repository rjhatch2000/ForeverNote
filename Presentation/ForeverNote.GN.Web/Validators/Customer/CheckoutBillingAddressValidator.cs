using ForeverNote.Core.Domain.Common;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Directory;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Models.Checkout;
using ForeverNote.Web.Validators.Common;
using System.Collections.Generic;

namespace ForeverNote.Web.Validators.Customer
{
    public class CheckoutBillingAddressValidator : BaseForeverNoteValidator<CheckoutBillingAddressModel>
    {
        public CheckoutBillingAddressValidator(
            IEnumerable<IValidatorConsumer<CheckoutBillingAddressModel>> validators,
            IEnumerable<IValidatorConsumer<Models.Common.AddressModel>> addressvalidators,
            ILocalizationService localizationService,
            IStateProvinceService stateProvinceService,
            AddressSettings addressSettings)
            : base(validators)
        {
            RuleFor(x => x.NewAddress).SetValidator(new AddressValidator(addressvalidators, localizationService, stateProvinceService, addressSettings));
        }
    }
}
