using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Models.Common;

namespace ForeverNote.Web.Areas.Admin.Models.Orders
{
    public partial class OrderAddressModel : BaseForeverNoteModel
    {
        public string OrderId { get; set; }
        public AddressModel Address { get; set; }
    }
}