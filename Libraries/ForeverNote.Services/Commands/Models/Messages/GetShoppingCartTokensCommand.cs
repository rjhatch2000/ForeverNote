using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Services.Messages.DotLiquidDrops;
using MediatR;
namespace ForeverNote.Services.Commands.Models.Messages
{
    public class GetShoppingCartTokensCommand : IRequest<LiquidShoppingCart>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public Language Language { get; set; }
        public string PersonalMessage { get; set; } = "";
        public string CustomerEmail { get; set; } = "";
    }
}
