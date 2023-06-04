using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Customers
{
    public partial class VendorReviewListModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.VendorReviews.List.CreatedOnFrom")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnFrom { get; set; }

        [ForeverNoteResourceDisplayName("Admin.VendorReviews.List.CreatedOnTo")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

        [ForeverNoteResourceDisplayName("Admin.VendorReviews.List.SearchText")]
        public string SearchText { get; set; }

        [ForeverNoteResourceDisplayName("Admin.VendorReviews.List.SearchVendor")]
        public string SearchVendorId { get; set; }

    }
}