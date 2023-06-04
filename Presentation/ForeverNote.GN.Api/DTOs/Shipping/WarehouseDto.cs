using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Api.DTOs.Shipping
{
    public partial class WarehouseDto : BaseApiEntityModel
    {
        public string Name { get; set; }
        public string AdminComment { get; set; }
    }
}
