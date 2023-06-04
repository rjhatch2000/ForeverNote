using ForeverNote.Services.Customers;
using ForeverNote.Services.Events.Web;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Events
{
    public class AddToCartEventHandler : INotificationHandler<AddToCartEvent>
    {
        private readonly ICustomerActionEventService _customerActionEventService;

        public AddToCartEventHandler(ICustomerActionEventService customerActionEventService)
        {
            _customerActionEventService = customerActionEventService;
        }

        public async Task Handle(AddToCartEvent notification, CancellationToken cancellationToken)
        {
            await _customerActionEventService.AddToCart(notification.ShoppingCartItem, notification.Product, notification.Customer);
        }
    }
}
