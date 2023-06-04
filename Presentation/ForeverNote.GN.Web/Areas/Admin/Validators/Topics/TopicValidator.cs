using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Topics;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Topics
{
    public class TopicValidator : BaseForeverNoteValidator<TopicModel>
    {
        public TopicValidator(
            IEnumerable<IValidatorConsumer<TopicModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.SystemName).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.Topics.Fields.SystemName.Required"));
        }
    }
}