using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class AddProductManufacturerCommand : IRequest<bool>
    {
        public ProductDto Product { get; set; }
        public ProductManufacturerDto Model { get; set; }
    }
}
