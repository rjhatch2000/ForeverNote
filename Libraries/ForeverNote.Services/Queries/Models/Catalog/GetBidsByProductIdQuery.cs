using ForeverNote.Core.Domain.Catalog;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Services.Queries.Models.Catalog
{
    public class GetBidsByProductIdQuery : IRequest<IList<Bid>>
    {
        public string ProductId { get; set; }
    }
}
