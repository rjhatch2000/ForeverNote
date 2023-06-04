using ForeverNote.Core.Domain.Catalog;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Services.Commands.Models.Catalog
{
    public class SendNotificationsToSubscribersCommand : IRequest<IList<BackInStockSubscription>>
    {
        public Product Product { get; set; }
        public string AttributeXml { get; set; }
        public string Warehouse { get; set; }
    }
}
