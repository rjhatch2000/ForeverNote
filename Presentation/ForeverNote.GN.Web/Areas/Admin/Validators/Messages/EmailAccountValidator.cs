using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Messages;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Messages
{
    public class EmailAccountValidator : BaseForeverNoteValidator<EmailAccountModel>
    {
        public EmailAccountValidator(
            IEnumerable<IValidatorConsumer<EmailAccountModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.EmailAccounts.Fields.Email"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Admin.Common.WrongEmail"));
            RuleFor(x => x.DisplayName).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.EmailAccounts.Fields.DisplayName"));
        }
    }
}