using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class AddProductTierPriceCommand : IRequest<bool>
    {
        public ProductDto Product { get; set; }
        public ProductTierPriceDto Model { get; set; }
    }
}
