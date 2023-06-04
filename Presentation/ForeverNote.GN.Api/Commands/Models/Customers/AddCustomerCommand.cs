using ForeverNote.Api.DTOs.Customers;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Customers
{
    public class AddCustomerCommand : IRequest<CustomerDto>
    {
        public CustomerDto Model { get; set; }
    }
}
