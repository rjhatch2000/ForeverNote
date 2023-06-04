using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Directory;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Orders;
using MediatR;

namespace ForeverNote.Web.Features.Models.Orders
{
    public class GetCustomerRewardPoints : IRequest<CustomerRewardPointsModel>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public Currency Currency { get; set; }
    }
}
