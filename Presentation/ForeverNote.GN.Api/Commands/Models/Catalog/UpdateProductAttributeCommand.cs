using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class UpdateProductAttributeCommand : IRequest<ProductAttributeDto>
    {
        public ProductAttributeDto Model { get; set; }
    }
}
