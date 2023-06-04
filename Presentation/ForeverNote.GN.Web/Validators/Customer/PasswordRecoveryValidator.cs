using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Models.Customer;
using System.Collections.Generic;

namespace ForeverNote.Web.Validators.Customer
{
    public class PasswordRecoveryValidator : BaseForeverNoteValidator<PasswordRecoveryModel>
    {
        public PasswordRecoveryValidator(
            IEnumerable<IValidatorConsumer<PasswordRecoveryModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Account.PasswordRecovery.Email.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
        }}
}