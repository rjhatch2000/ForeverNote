using ForeverNote.Web.Models.Orders;
using MediatR;

namespace ForeverNote.Web.Features.Models.Orders
{
    public class GetOrderDetails : IRequest<OrderDetailsModel>
    {
        public Core.Domain.Orders.Order Order { get; set; }
        public Core.Domain.Localization.Language Language { get; set; }
    }
}
