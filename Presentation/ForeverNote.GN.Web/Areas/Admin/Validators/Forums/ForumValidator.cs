using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Forums;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Forums
{
    public class ForumValidator : BaseForeverNoteValidator<ForumModel>
    {
        public ForumValidator(
            IEnumerable<IValidatorConsumer<ForumModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.Forums.Forum.Fields.Name.Required"));
            RuleFor(x => x.ForumGroupId).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.Forums.Forum.Fields.ForumGroupId.Required"));
        }
    }
}