using ForeverNote.Core.Domain.Common;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Directory;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Models.Customer;
using ForeverNote.Web.Validators.Common;
using System.Collections.Generic;

namespace ForeverNote.Web.Validators.Customer
{
    public class CustomerAddressEditValidator : BaseForeverNoteValidator<CustomerAddressEditModel>
    {
        public CustomerAddressEditValidator(
            IEnumerable<IValidatorConsumer<CustomerAddressEditModel>> validators,
            IEnumerable<IValidatorConsumer<Models.Common.AddressModel>> addressvalidators,
            ILocalizationService localizationService,
            IStateProvinceService stateProvinceService,
            AddressSettings addressSettings)
            : base(validators)
        {
            RuleFor(x => x.Address).SetValidator(new AddressValidator(addressvalidators, localizationService, stateProvinceService, addressSettings));
        }
    }
}
