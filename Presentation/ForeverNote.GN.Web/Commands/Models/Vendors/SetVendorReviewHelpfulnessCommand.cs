using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Vendors;
using MediatR;

namespace ForeverNote.Web.Commands.Models.Vendors
{
    public class SetVendorReviewHelpfulnessCommand : IRequest<VendorReview>
    {
        public Customer Customer { get; set; }
        public Vendor Vendor { get; set; }
        public VendorReview Review { get; set; }
        public bool Washelpful { get; set; }
    }
}
