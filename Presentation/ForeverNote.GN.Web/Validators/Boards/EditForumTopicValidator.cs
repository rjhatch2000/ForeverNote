using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Models.Boards;
using System.Collections.Generic;

namespace ForeverNote.Web.Validators.Boards
{
    public class EditForumTopicValidator : BaseForeverNoteValidator<EditForumTopicModel>
    {
        public EditForumTopicValidator(
            IEnumerable<IValidatorConsumer<EditForumTopicModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Subject).NotEmpty().WithMessage(localizationService.GetResource("Forum.TopicSubjectCannotBeEmpty"));
            RuleFor(x => x.Text).NotEmpty().WithMessage(localizationService.GetResource("Forum.TextCannotBeEmpty"));
        }
    }
}