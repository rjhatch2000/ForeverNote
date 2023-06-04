using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Directory;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Checkout;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Web.Features.Models.Checkout
{
    public class GetBillingAddress : IRequest<CheckoutBillingAddressModel>
    {
        public IList<ShoppingCartItem> Cart { get; set; }

        public string SelectedCountryId { get; set; } = null;
        public bool PrePopulateNewAddressWithCustomerFields { get; set; } = false;
        public string OverrideAttributesXml { get; set; } = "";
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public Currency Currency { get; set; }
        public Language Language { get; set; }
    }
}
