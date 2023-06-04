using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Templates;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Templates
{
    public class TopicTemplateValidator : BaseForeverNoteValidator<TopicTemplateModel>
    {
        public TopicTemplateValidator(
            IEnumerable<IValidatorConsumer<TopicTemplateModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.Templates.Topic.Name.Required"));
            RuleFor(x => x.ViewPath).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.Templates.Topic.ViewPath.Required"));
        }
    }
}