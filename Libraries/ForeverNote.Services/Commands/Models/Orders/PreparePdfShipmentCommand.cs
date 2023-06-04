using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Shipping;
using iTextSharp.text;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Orders
{
    public class PreparePdfShipmentCommand : IRequest<bool>
    {
        public Document Doc { get; set; }
        public Shipment Shipment { get; set; }
        public string LanguageId { get; set; } = "";
        public Language Language { get; set; }
    }
}
