using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Models.Orders;
using System.Collections.Generic;

namespace ForeverNote.Web.Validators.Customer
{
    public class AddOrderNoteValidator : BaseForeverNoteValidator<AddOrderNoteModel>
    {
        public AddOrderNoteValidator(
            IEnumerable<IValidatorConsumer<AddOrderNoteModel>> validators,
            ILocalizationService localizationService)
            : base(validators)
        {
            RuleFor(x => x.Note).NotEmpty().WithMessage(localizationService.GetResource("OrderNote.Fields.Title.Required"));
        }
    }
}
