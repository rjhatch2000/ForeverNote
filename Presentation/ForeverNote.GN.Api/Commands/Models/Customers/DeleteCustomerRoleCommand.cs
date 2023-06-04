using ForeverNote.Api.DTOs.Customers;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Customers
{
    public class DeleteCustomerRoleCommand : IRequest<bool>
    {
        public CustomerRoleDto Model { get; set; }
    }
}
