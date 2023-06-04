using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Models.Boards;
using System.Collections.Generic;

namespace ForeverNote.Web.Validators.Boards
{
    public class EditForumPostValidator : BaseForeverNoteValidator<EditForumPostModel>
    {
        public EditForumPostValidator(
            IEnumerable<IValidatorConsumer<EditForumPostModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Text).NotEmpty().WithMessage(localizationService.GetResource("Forum.TextCannotBeEmpty"));
        }
    }
}