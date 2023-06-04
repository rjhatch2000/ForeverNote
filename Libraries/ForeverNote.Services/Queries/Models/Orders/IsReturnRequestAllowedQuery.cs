using ForeverNote.Core.Domain.Orders;
using MediatR;

namespace ForeverNote.Services.Queries.Models.Orders
{
    public class IsReturnRequestAllowedQuery : IRequest<bool>
    {
        public Order Order { get; set; }
    }
}
