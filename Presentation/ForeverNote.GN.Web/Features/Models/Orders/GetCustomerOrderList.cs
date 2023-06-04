using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Orders;
using MediatR;

namespace ForeverNote.Web.Features.Models.Orders
{
    public class GetCustomerOrderList : IRequest<CustomerOrderListModel>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public Language Language { get; set; }
    }
}
