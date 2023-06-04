using ForeverNote.Web.Models.Customer;
using MediatR;
using System;

namespace ForeverNote.Web.Features.Models.Customers
{
    public class GetUserAgreement : IRequest<UserAgreementModel>
    {
        public Guid OrderItemId { get; set; }
    }
}
