using ForeverNote.Core.Domain.Catalog;
using MediatR;

namespace ForeverNote.Web.Features.Models.Catalog
{
    public class GetProductAttribute : IRequest<ProductAttribute>
    {
        public string Id { get; set; }
    }
}
