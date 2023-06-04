using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Forums;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Forums
{
    public class ForumGroupValidator : BaseForeverNoteValidator<ForumGroupModel>
    {
        public ForumGroupValidator(
            IEnumerable<IValidatorConsumer<ForumGroupModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.Forums.ForumGroup.Fields.Name.Required"));
        }
    }
}