using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class UpdateProductStockCommand : IRequest<bool>
    {
        public ProductDto Product { get; set; }
        public string WarehouseId { get; set; }
        public int Stock { get; set; }
    }
}
