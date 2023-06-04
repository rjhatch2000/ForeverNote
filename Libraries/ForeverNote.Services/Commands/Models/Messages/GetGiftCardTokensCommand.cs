using ForeverNote.Core.Domain.Orders;
using ForeverNote.Services.Messages.DotLiquidDrops;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Messages
{
    public class GetGiftCardTokensCommand : IRequest<LiquidGiftCard>
    {
        public GiftCard GiftCard { get; set; }
    }
}
