using MediatR;

namespace ForeverNote.Api.Commands.Models.Customers
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public string Email { get; set; }
    }
}
