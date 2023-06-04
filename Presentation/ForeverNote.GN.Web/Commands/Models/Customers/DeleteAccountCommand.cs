using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Stores;
using MediatR;

namespace ForeverNote.Web.Commands.Models.Customers
{
    public class DeleteAccountCommand : IRequest<bool>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
    }
}
