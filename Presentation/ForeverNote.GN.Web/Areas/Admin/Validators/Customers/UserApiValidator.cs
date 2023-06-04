using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Customers;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Models.Customers;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Customers
{
    public class UserApiValidator : BaseForeverNoteValidator<UserApiModel>
    {
        public UserApiValidator(
            IEnumerable<IValidatorConsumer<UserApiModel>> validators,
            ILocalizationService localizationService, ICustomerService customerService)
            : base(validators)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.UserApi.Email.Required"));
            RuleFor(x => x).MustAsync(async (x, y, context) =>
            {
                if (!string.IsNullOrEmpty(x.Email))
                {
                    var customer = await customerService.GetCustomerByEmail(x.Email.ToLowerInvariant());
                    if (customer != null && customer.Active && !customer.IsSystemAccount)
                        return true;
                }
                return false;
            }).WithMessage(localizationService.GetResource("Admin.System.UserApi.Email.CustomerNotExist")); ;
        }
    }
}