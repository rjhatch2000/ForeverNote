using ForeverNote.Core.Domain.Customers;
using ForeverNote.Web.Models.Customer;
using MediatR;

namespace ForeverNote.Web.Features.Models.Customers
{
    public class GetPasswordRecoveryConfirm : IRequest<PasswordRecoveryConfirmModel>
    {
        public Customer Customer { get; set; }
        public string Token { get; set; }
    }
}
