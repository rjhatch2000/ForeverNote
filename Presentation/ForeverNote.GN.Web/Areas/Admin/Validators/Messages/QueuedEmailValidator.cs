using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Messages;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Messages
{
    public class QueuedEmailValidator : BaseForeverNoteValidator<QueuedEmailModel>
    {
        public QueuedEmailValidator(
            IEnumerable<IValidatorConsumer<QueuedEmailModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.From).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.QueuedEmails.Fields.From.Required"));
            RuleFor(x => x.To).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.QueuedEmails.Fields.To.Required"));

            RuleFor(x => x.SentTries).NotNull().WithMessage(localizationService.GetResource("Admin.System.QueuedEmails.Fields.SentTries.Required"))
                                    .InclusiveBetween(0, 99999).WithMessage(localizationService.GetResource("Admin.System.QueuedEmails.Fields.SentTries.Range"));

        }
    }
}