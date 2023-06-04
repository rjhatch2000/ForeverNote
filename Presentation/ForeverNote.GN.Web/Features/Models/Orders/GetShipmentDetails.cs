using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Shipping;
using ForeverNote.Web.Models.Orders;
using MediatR;

namespace ForeverNote.Web.Features.Models.Orders
{
    public class GetShipmentDetails : IRequest<ShipmentDetailsModel>
    {
        public Shipment Shipment { get; set; }
        public Order Order { get; set; }
        public Customer Customer { get; set; }
        public Language Language { get; set; }
    }
}
