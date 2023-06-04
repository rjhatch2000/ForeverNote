using FluentValidation;
using ForeverNote.Core.Domain.Vendors;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Directory;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Models.Vendors;
using ForeverNote.Web.Validators.Common;
using System.Collections.Generic;

namespace ForeverNote.Web.Validators.Vendors
{
    public class VendorInfoValidator : BaseForeverNoteValidator<VendorInfoModel>
    {
        public VendorInfoValidator(
            IEnumerable<IValidatorConsumer<VendorInfoModel>> validators,
            IEnumerable<IValidatorConsumer<VendorAddressModel>> addressvalidators,
            ILocalizationService localizationService, 
            IStateProvinceService stateProvinceService, VendorSettings addressSettings)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Account.VendorInfo.Name.Required"));
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Account.VendorInfo.Email.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            RuleFor(x => x.Address).SetValidator(new VendorAddressValidator(addressvalidators, localizationService, stateProvinceService, addressSettings));
        }
    }
}