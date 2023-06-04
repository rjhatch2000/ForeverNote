using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class DeleteProductAttributeCommand : IRequest<bool>
    {
        public ProductAttributeDto Model { get; set; }
    }
}
