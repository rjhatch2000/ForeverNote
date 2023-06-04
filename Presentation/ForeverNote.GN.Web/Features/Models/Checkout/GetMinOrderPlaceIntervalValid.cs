using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Stores;
using MediatR;

namespace ForeverNote.Web.Features.Models.Checkout
{
    public class GetMinOrderPlaceIntervalValid : IRequest<bool>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
    }
}
