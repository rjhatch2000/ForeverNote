using MediatR;

namespace ForeverNote.Core.Domain.Vendors
{
    /// <summary>
    /// Vendor review approved event
    /// </summary>
    public class VendorReviewApprovedEvent : INotification
    {
        public VendorReviewApprovedEvent(VendorReview vendorReview)
        {
            this.VendorReview = vendorReview;
        }

        /// <summary>
        /// Vendor review
        /// </summary>
        public VendorReview VendorReview { get; private set; }
    }

}