using ForeverNote.Core.Domain.Catalog;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Catalog
{
    public class SendQuantityBelowStoreOwnerNotificationCommand : IRequest<bool>
    {
        public Product Product { get; set; }
        public ProductAttributeCombination ProductAttributeCombination { get; set; }

    }
}
