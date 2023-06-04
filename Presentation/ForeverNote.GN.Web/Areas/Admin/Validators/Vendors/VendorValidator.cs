using FluentValidation;
using ForeverNote.Web.Framework.Extensions;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Vendors;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Vendors
{
    public class VendorValidator : BaseForeverNoteValidator<VendorModel>
    {
        public VendorValidator(
            IEnumerable<IValidatorConsumer<VendorModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Vendors.Fields.Name.Required"));
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Admin.Vendors.Fields.Email.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Admin.Common.WrongEmail"));
            RuleFor(x => x.PageSizeOptions)
                .Must(FluentValidationUtilities.PageSizeOptionsValidator)
                .WithMessage(localizationService.GetResource("Admin.Vendors.Fields.PageSizeOptions.ShouldHaveUniqueItems"));
            RuleFor(x => x.Commission)
                .Must(IsCommissionValid)
                .WithMessage(localizationService.GetResource("Admin.Vendors.Fields.Commission.IsCommissionValid"));
                
        }
        
        private bool IsCommissionValid(decimal commission)
        {
            if (commission < 0 || commission > 100)
                return false;

            return true;
        }
    }
}