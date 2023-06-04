using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Models.PrivateMessages;
using System.Collections.Generic;

namespace ForeverNote.Web.Validators.PrivateMessages
{
    public class SendPrivateMessageValidator : BaseForeverNoteValidator<SendPrivateMessageModel>
    {
        public SendPrivateMessageValidator(
            IEnumerable<IValidatorConsumer<SendPrivateMessageModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Subject).NotEmpty().WithMessage(localizationService.GetResource("PrivateMessages.SubjectCannotBeEmpty"));
            RuleFor(x => x.Message).NotEmpty().WithMessage(localizationService.GetResource("PrivateMessages.MessageCannotBeEmpty"));
        }
    }
}