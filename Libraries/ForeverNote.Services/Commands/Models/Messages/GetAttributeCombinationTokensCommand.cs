using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Services.Messages.DotLiquidDrops;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Messages
{
    public class GetAttributeCombinationTokensCommand : IRequest<LiquidAttributeCombination>
    {
        public Product Product { get; set; }
        public ProductAttributeCombination Combination { get; set; }
    }
}
