using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Localization;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Catalog
{
    public class SendOutBidCustomerNotificationCommand : IRequest<bool>
    {
        public Product Product { get; set; }
        public Bid Bid { get; set; }
        public Language Language { get; set; }
    }
}
