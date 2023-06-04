using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Customers;
using ForeverNote.Web.Areas.Admin.Validators.Common;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Customers
{
    public class CustomerAddressValidator : BaseForeverNoteValidator<CustomerAddressModel>
    {
        public CustomerAddressValidator(
            IEnumerable<IValidatorConsumer<CustomerAddressModel>> validators,
            IEnumerable<IValidatorConsumer<Models.Common.AddressModel>> addressvalidators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Address).SetValidator(new AddressValidator(addressvalidators, localizationService));
        }
    }
}
