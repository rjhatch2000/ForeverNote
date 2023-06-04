using System.Collections.Generic;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Directory;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Checkout;
using MediatR;

namespace ForeverNote.Web.Features.Models.Checkout
{
    public class GetPaymentMethod : IRequest<CheckoutPaymentMethodModel>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public Currency Currency { get; set; }
        public Language Language { get; set; }
        public IList<ShoppingCartItem> Cart { get; set; } 
        public string FilterByCountryId { get; set; }
    }
}
