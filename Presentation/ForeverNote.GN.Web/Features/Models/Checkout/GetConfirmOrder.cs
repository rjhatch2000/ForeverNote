using ForeverNote.Core.Domain.Directory;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Web.Models.Checkout;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Web.Features.Models.Checkout
{
    public class GetConfirmOrder : IRequest<CheckoutConfirmModel>
    {
        public IList<ShoppingCartItem> Cart { get; set; }
        public Currency Currency { get; set; }
    }
}
