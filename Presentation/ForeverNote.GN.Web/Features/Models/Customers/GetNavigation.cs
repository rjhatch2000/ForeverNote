using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Core.Domain.Vendors;
using ForeverNote.Web.Models.Customer;
using MediatR;

namespace ForeverNote.Web.Features.Models.Customers
{
    public class GetNavigation : IRequest<CustomerNavigationModel>
    {
        public int SelectedTabId { get; set; } = 0;
        public Customer Customer { get; set; }
        public Language Language { get; set; }
        public Store Store { get; set; }
        public Vendor Vendor { get; set; }
    }
}
