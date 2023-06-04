using FluentValidation;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Models.Customer;
using System.Collections.Generic;

namespace ForeverNote.Web.Validators.Customer
{
    public class DeleteAccountValidator : BaseForeverNoteValidator<DeleteAccountModel>
    {
        public DeleteAccountValidator(
            IEnumerable<IValidatorConsumer<DeleteAccountModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage(localizationService.GetResource("Account.DeleteAccount.Fields.Password.Required"));
        }}
}