using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.ShoppingCart
{
    public partial class ShoppingCartModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.CurrentCarts.Customer")]
        public string CustomerId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.CurrentCarts.Customer")]
        public string CustomerEmail { get; set; }

        [ForeverNoteResourceDisplayName("Admin.CurrentCarts.TotalItems")]
        public int TotalItems { get; set; }
    }
}