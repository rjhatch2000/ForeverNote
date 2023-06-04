using ForeverNote.Core.Domain.Catalog;
using MediatR;

namespace ForeverNote.Services.Queries.Models.Catalog
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public string Id { get; set; }
    }
}
