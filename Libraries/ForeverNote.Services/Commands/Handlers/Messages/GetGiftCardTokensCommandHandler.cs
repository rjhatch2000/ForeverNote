using ForeverNote.Services.Catalog;
using ForeverNote.Services.Commands.Models.Messages;
using ForeverNote.Services.Messages.DotLiquidDrops;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Commands.Handlers.Messages
{
    public class GetGiftCardTokensCommandHandler : IRequestHandler<GetGiftCardTokensCommand, LiquidGiftCard>
    {
        private readonly IPriceFormatter _priceFormatter;

        public GetGiftCardTokensCommandHandler(IPriceFormatter priceFormatter)
        {
            _priceFormatter = priceFormatter;
        }

        public async Task<LiquidGiftCard> Handle(GetGiftCardTokensCommand request, CancellationToken cancellationToken)
        {
            var liquidGiftCart = new LiquidGiftCard(request.GiftCard);
            liquidGiftCart.Amount = _priceFormatter.FormatPrice(request.GiftCard.Amount, true, false);
            return await Task.FromResult(liquidGiftCart);
        }
    }
}
