using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Api.DTOs.Catalog
{
    public partial class ProductWarehouseInventoryDto : BaseApiEntityModel
    {
        public string WarehouseId { get; set; }
        public int StockQuantity { get; set; }
        public int ReservedQuantity { get; set; }
    }
}
