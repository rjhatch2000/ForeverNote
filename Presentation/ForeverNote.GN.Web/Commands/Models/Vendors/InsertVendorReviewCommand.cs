using ForeverNote.Core.Domain.Stores;
using ForeverNote.Core.Domain.Vendors;
using ForeverNote.Web.Models.Vendors;
using MediatR;

namespace ForeverNote.Web.Commands.Models.Vendors
{
    public class InsertVendorReviewCommand : IRequest<VendorReview>
    {
        public Vendor Vendor { get; set; }
        public Store Store { get; set; }
        public VendorReviewsModel Model { get; set; }
    }
}
