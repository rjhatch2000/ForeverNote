using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Web.Models.Orders;
using MediatR;

namespace ForeverNote.Web.Features.Models.Orders
{
    public class GetReturnRequestDetails : IRequest<ReturnRequestDetailsModel>
    {
        public ReturnRequest ReturnRequest { get; set; }
        public Order Order { get; set; }
        public Language Language { get; set; }
    }
}
