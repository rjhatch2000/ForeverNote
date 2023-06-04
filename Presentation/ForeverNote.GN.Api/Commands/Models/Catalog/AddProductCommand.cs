using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class AddProductCommand : IRequest<ProductDto>
    {
        public ProductDto Model { get; set; }
    }
}
