using ForeverNote.Core.Domain.Customers;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Orders
{
    public class CalculateRewardPointsCommand : IRequest<int>
    {
        public Customer Customer { get; set; }
        public decimal Amount { get; set; }
    }
}
