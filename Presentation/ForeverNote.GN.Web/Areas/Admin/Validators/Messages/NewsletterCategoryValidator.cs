using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Messages;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Messages
{
    public class NewsletterCategoryValidator : BaseForeverNoteValidator<NewsletterCategoryModel>
    {
        public NewsletterCategoryValidator(
            IEnumerable<IValidatorConsumer<NewsletterCategoryModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Promotions.NewsletterCategory.Fields.Name.Required"));
        }
    }
}