using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using System;

namespace ForeverNote.Web.Areas.Admin.Models.ShoppingCart
{
    public partial class ShoppingCartItemModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.CurrentCarts.Store")]
        public string Store { get; set; }
        [ForeverNoteResourceDisplayName("Admin.CurrentCarts.Product")]
        public string ProductId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.CurrentCarts.Product")]
        public string ProductName { get; set; }
        public string AttributeInfo { get; set; }

        [ForeverNoteResourceDisplayName("Admin.CurrentCarts.UnitPrice")]
        public string UnitPrice { get; set; }
        [ForeverNoteResourceDisplayName("Admin.CurrentCarts.Quantity")]
        public int Quantity { get; set; }
        [ForeverNoteResourceDisplayName("Admin.CurrentCarts.Total")]
        public string Total { get; set; }
        [ForeverNoteResourceDisplayName("Admin.CurrentCarts.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }
    }
}