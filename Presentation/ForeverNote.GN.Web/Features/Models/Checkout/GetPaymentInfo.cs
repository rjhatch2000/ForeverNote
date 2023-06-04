using ForeverNote.Services.Payments;
using ForeverNote.Web.Models.Checkout;
using MediatR;

namespace ForeverNote.Web.Features.Models.Checkout
{
    public class GetPaymentInfo : IRequest<CheckoutPaymentInfoModel>
    {
        public IPaymentMethod PaymentMethod { get; set; }
    }
}
