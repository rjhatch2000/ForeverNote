using ForeverNote.Services.Users;
using MediatR;
using System.Threading.Tasks;

namespace ForeverNote.Services.Events.Web
{
    public static class WebEventsExtensions
    {
        public static async Task UserRegistrationEvent<C, R>(this IMediator eventPublisher, C result, R request) where C : UserRegistrationResult where R : UserRegistrationRequest
        {
            await eventPublisher.Publish(new UserRegistrationEvent<C, R>(result, request));
        }
    }
}
