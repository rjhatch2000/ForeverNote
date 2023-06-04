using ForeverNote.Core.Domain.Orders;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Orders
{
    public class ReduceRewardPointsCommand : IRequest<bool>
    {
        public Order Order { get; set; }
    }
}
