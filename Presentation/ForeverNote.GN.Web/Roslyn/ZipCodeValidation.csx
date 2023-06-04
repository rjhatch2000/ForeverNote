#! "netcoreapp3.1"
#r "ForeverNote.Core"
#r "ForeverNote.Web.Framework"
#r "ForeverNote.Services"
#r "ForeverNote.Web"

using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Events;
using ForeverNote.Web.Models.Common;
using System.Threading.Tasks;

/* Sample code to validate ZIP Code field in the Address */
public class ZipCodeValidation : IValidatorConsumer<AddressModel>
{
    public void AddRules(BaseForeverNoteValidator<AddressModel> validator)
    {
        validator.RuleFor(x => x.ZipPostalCode).Matches(@"^[0-9]{2}\-[0-9]{3}$")
            .WithMessage("Provided zip code is invalid");
    }
}
