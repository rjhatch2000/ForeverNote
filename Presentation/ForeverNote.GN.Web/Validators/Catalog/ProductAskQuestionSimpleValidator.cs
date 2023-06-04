using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Models.Catalog;
using System.Collections.Generic;

namespace ForeverNote.Web.Validators.Catalog
{
    public class ProductAskQuestionSimpleValidator : BaseForeverNoteValidator<ProductAskQuestionSimpleModel>
    {
        public ProductAskQuestionSimpleValidator(
            IEnumerable<IValidatorConsumer<ProductAskQuestionSimpleModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.AskQuestionEmail).NotEmpty().WithMessage(localizationService.GetResource("Products.AskQuestion.Email.Required"));
            RuleFor(x => x.AskQuestionEmail).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            RuleFor(x => x.AskQuestionMessage).NotEmpty().WithMessage(localizationService.GetResource("Products.AskQuestion.Message.Required"));
            RuleFor(x => x.AskQuestionFullName).NotEmpty().WithMessage(localizationService.GetResource("Products.AskQuestion.FullName.Required"));
        }
    }
}