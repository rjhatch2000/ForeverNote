using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Customer;
using MediatR;

namespace ForeverNote.Web.Features.Models.Customers
{
    public class GetAddressList : IRequest<CustomerAddressListModel>
    {
        public Store Store { get; set; }
        public Customer Customer { get; set; }
        public Language Language { get; set; }
    }
}
