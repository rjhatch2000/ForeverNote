using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Orders
{
    public partial class GiftCardListModel : BaseForeverNoteModel
    {
        public GiftCardListModel()
        {
            ActivatedList = new List<SelectListItem>();
        }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.List.CouponCode")]
        
        public string CouponCode { get; set; }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.List.RecipientName")]
        
        public string RecipientName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.List.Activated")]
        public int ActivatedId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.GiftCards.List.Activated")]
        public IList<SelectListItem> ActivatedList { get; set; }
    }
}