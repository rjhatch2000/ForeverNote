using ForeverNote.Api.DTOs.Customers;
using MediatR;

namespace ForeverNote.Api.Queries.Models.Customers
{
    public class GetCustomerQuery : IRequest<CustomerDto>
    {
        public string Email { get; set; }
    }
}
