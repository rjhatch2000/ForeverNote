using ForeverNote.Core.Domain.Customers;
using ForeverNote.Web.Models.Customer;
using MediatR;

namespace ForeverNote.Web.Features.Models.Customers
{
    public class GetAvatar : IRequest<CustomerAvatarModel>
    {
        public Customer Customer { get; set; }
    }
}
