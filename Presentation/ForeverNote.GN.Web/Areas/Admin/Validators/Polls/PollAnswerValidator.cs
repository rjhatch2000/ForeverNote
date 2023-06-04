using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Polls;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Polls
{
    public class PollAnswerValidator : BaseForeverNoteValidator<PollAnswerModel>
    {
        public PollAnswerValidator(
            IEnumerable<IValidatorConsumer<PollAnswerModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.Polls.Answers.Fields.Name.Required"));
        }
    }
}