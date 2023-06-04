using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Validators.Vendors;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Vendors
{
    public partial class VendorReviewOverviewModel : BaseForeverNoteModel
    {
        public string VendorId { get; set; }

        public int RatingSum { get; set; }

        public int TotalReviews { get; set; }

        public bool AllowCustomerReviews { get; set; }
    }

    [Validator(typeof(VendorReviewsValidator))]
    public partial class VendorReviewsModel : BaseForeverNoteModel
    {
        public VendorReviewsModel()
        {
            Items = new List<VendorReviewModel>();
            AddVendorReview = new AddVendorReviewModel();
        }
        public string VendorId { get; set; }

        public string VendorName { get; set; }

        public string VendorSeName { get; set; }

        public IList<VendorReviewModel> Items { get; set; }
        public AddVendorReviewModel AddVendorReview { get; set; }
    }

    public partial class VendorReviewModel : BaseForeverNoteEntityModel
    {
        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public bool AllowViewingProfiles { get; set; }

        public string Title { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public VendorReviewHelpfulnessModel Helpfulness { get; set; }

        public string WrittenOnStr { get; set; }
    }

    public partial class VendorReviewHelpfulnessModel : BaseForeverNoteModel
    {
        public string VendorReviewId { get; set; }
        public string VendorId { get; set; }

        public int HelpfulYesTotal { get; set; }

        public int HelpfulNoTotal { get; set; }
    }

    public partial class AddVendorReviewModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Reviews.Fields.Title")]
        public string Title { get; set; }

        [ForeverNoteResourceDisplayName("Reviews.Fields.ReviewText")]
        public string ReviewText { get; set; }

        [ForeverNoteResourceDisplayName("Reviews.Fields.Rating")]
        public int Rating { get; set; }

        public bool DisplayCaptcha { get; set; }

        public bool CanCurrentCustomerLeaveReview { get; set; }
        public bool SuccessfullyAdded { get; set; }
        public string Result { get; set; }
    }
}