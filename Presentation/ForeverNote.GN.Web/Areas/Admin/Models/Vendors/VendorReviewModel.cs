using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Vendors;
using System;

namespace ForeverNote.Web.Areas.Admin.Models.Vendors
{
    [Validator(typeof(VendorReviewValidator))]
    public partial class VendorReviewModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.VendorReviews.Fields.Vendor")]
        public string VendorId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.VendorReviews.Fields.Vendor")]
        public string VendorName { get; set; }

        public string Ids
        {
            get
            {
                return Id.ToString() + ":" + VendorId;
            }
        }
        [ForeverNoteResourceDisplayName("Admin.VendorReviews.Fields.Customer")]
        public string CustomerId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.VendorReviews.Fields.Customer")]
        public string CustomerInfo { get; set; }

        [ForeverNoteResourceDisplayName("Admin.VendorReviews.Fields.Title")]
        public string Title { get; set; }

        [ForeverNoteResourceDisplayName("Admin.VendorReviews.Fields.ReviewText")]
        public string ReviewText { get; set; }

        [ForeverNoteResourceDisplayName("Admin.VendorReviews.Fields.Rating")]
        public int Rating { get; set; }

        [ForeverNoteResourceDisplayName("Admin.VendorReviews.Fields.IsApproved")]
        public bool IsApproved { get; set; }

        [ForeverNoteResourceDisplayName("Admin.VendorReviews.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }
}
