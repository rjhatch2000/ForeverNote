using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Vendors;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Vendors
{
    public class VendorReviewValidator : BaseForeverNoteValidator<VendorReviewModel>
    {
        public VendorReviewValidator(
            IEnumerable<IValidatorConsumer<VendorReviewModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(localizationService.GetResource("Admin.VendorReviews.Fields.Title.Required"));
            RuleFor(x => x.ReviewText).NotEmpty().WithMessage(localizationService.GetResource("Admin.VendorReviews.Fields.ReviewText.Required"));
        }
    }
}
