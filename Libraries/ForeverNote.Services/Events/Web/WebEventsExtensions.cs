using ForeverNote.Services.Customers;
using MediatR;
using System.Threading.Tasks;

namespace ForeverNote.Services.Events.Web
{
    public static class WebEventsExtensions
    {
        public static async Task CustomerRegistrationEvent<C, R>(this IMediator eventPublisher, C result, R request) where C : CustomerRegistrationResult where R : CustomerRegistrationRequest
        {
            await eventPublisher.Publish(new CustomerRegistrationEvent<C, R>(result, request));
        }
    }
}
