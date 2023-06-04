using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Orders;
using MediatR;

namespace ForeverNote.Web.Features.Models.Orders
{
    public class GetReturnRequest : IRequest<ReturnRequestModel>
    {
        public Order Order { get; set; }
        public Language Language { get; set; }
        public Store Store { get; set; }
    }
}
