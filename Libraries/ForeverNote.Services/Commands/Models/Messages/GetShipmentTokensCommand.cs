using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Shipping;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Services.Messages.DotLiquidDrops;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Messages
{
    public class GetShipmentTokensCommand : IRequest<LiquidShipment>
    {
        public Shipment Shipment { get; set; }
        public Order Order { get; set; }
        public Store Store { get; set; }
        public Language Language { get; set; }
    }
}
