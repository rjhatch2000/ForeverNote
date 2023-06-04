using ForeverNote.Core.Domain.Customers;
using ForeverNote.Web.Models.Boards;
using MediatR;

namespace ForeverNote.Web.Features.Models.Boards
{
    public class GetCustomerForumSubscriptions : IRequest<CustomerForumSubscriptionsModel>
    {
        public Customer Customer { get; set; }
        public int PageIndex { get; set; }
    }
}
