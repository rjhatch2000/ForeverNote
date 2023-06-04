using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Directory;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Core.Domain.Tax;
using ForeverNote.Web.Models.ShoppingCart;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Web.Features.Models.ShoppingCart
{
    public class GetShoppingCart : IRequest<ShoppingCartModel>
    {
        public Customer Customer { get; set; }
        public Language Language { get; set; }
        public Currency Currency { get; set; }
        public Store Store { get; set; }
        public TaxDisplayType TaxDisplayType { get; set; }
        public IList<ShoppingCartItem> Cart { get; set; }
        public bool IsEditable { get; set; } = true;
        public bool ValidateCheckoutAttributes { get; set; } = false;
        public bool SetEstimateShippingDefaultAddress { get; set; } = true;
        public bool PrepareAndDisplayOrderReviewData { get; set; } = false;
    }
}
