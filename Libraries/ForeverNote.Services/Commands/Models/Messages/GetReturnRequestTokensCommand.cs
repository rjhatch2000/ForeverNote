using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Services.Messages.DotLiquidDrops;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Messages
{
    public class GetReturnRequestTokensCommand : IRequest<LiquidReturnRequest>
    {
        public ReturnRequest ReturnRequest { get; set; }
        public Store Store { get; set; }
        public Order Order { get; set; }
        public Language Language { get; set; }
        public ReturnRequestNote ReturnRequestNote { get; set; } = null;
    }
}
