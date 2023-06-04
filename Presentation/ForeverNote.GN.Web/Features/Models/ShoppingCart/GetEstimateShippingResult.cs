using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Directory;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.ShoppingCart;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Web.Features.Models.ShoppingCart
{
    public class GetEstimateShippingResult : IRequest<EstimateShippingResultModel>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public Currency Currency { get; set; }

        public IList<ShoppingCartItem> Cart { get; set; }
        public string CountryId { get; set; }
        public string StateProvinceId { get; set; }
        public string ZipPostalCode { get; set; }
    }
}
