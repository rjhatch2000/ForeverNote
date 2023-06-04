using ForeverNote.Api.DTOs.Customers;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Customers
{
    public class AddCustomerRoleCommand : IRequest<CustomerRoleDto>
    {
        public CustomerRoleDto Model { get; set; }
    }
}
