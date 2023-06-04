using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Catalog;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Catalog
{
    public class ProductReviewValidator : BaseForeverNoteValidator<ProductReviewModel>
    {
        public ProductReviewValidator(
            IEnumerable<IValidatorConsumer<ProductReviewModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(localizationService.GetResource("Admin.Catalog.ProductReviews.Fields.Title.Required"));
            RuleFor(x => x.ReviewText).NotEmpty().WithMessage(localizationService.GetResource("Admin.Catalog.ProductReviews.Fields.ReviewText.Required"));
        }}
}