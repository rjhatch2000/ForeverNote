using ForeverNote.Core.Domain.Vendors;
using ForeverNote.Web.Models.Vendors;
using MediatR;

namespace ForeverNote.Web.Features.Models.Vendors
{
    public class GetVendorReviews : IRequest<VendorReviewsModel>
    {
        public Vendor Vendor { get; set; }
    }
}
