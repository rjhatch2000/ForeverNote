using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Vendors;
using ForeverNote.Services.Messages.DotLiquidDrops;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Messages
{
    public class GetVendorTokensCommand : IRequest<LiquidVendor>
    {
        public Vendor Vendor { get; set; }
        public Language Language { get; set; }
    }
}
