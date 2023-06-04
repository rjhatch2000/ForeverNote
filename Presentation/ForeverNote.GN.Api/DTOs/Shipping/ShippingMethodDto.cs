using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Api.DTOs.Shipping
{
    public partial class ShippingMethodDto : BaseApiEntityModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}
