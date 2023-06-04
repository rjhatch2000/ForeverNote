using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class UpdateProductSpecificationCommand : IRequest<bool>
    {
        public ProductDto Product { get; set; }
        public ProductSpecificationAttributeDto Model { get; set; }
    }
}
