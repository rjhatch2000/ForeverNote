using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Core.Domain.Vendors;
using ForeverNote.Services.Messages.DotLiquidDrops;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Messages
{
    public class GetOrderTokensCommand : IRequest<LiquidOrder>
    {
        public Order Order { get; set; }
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public OrderNote OrderNote { get; set; } = null;
        public Vendor Vendor { get; set; } = null;
        public decimal RefundedAmount { get; set; } = 0;
    }
}
