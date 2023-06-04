using FluentValidation;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Orders;
using ForeverNote.Web.Areas.Admin.Models.Orders;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Orders
{
    public class GiftCardValidator : BaseForeverNoteValidator<GiftCardModel>
    {
        public GiftCardValidator(
            IEnumerable<IValidatorConsumer<GiftCardModel>> validators,
            ILocalizationService localizationService, IGiftCardService giftCardService)
            : base(validators)
        {
            RuleFor(x => x.GiftCardCouponCode).NotEmpty().WithMessage(localizationService.GetResource("Admin.GiftCards.Fields.GiftCardCouponCode.Required"));
        }
    }
}