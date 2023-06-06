using ForeverNote.Services.Users;
using MediatR;

namespace ForeverNote.Services.Events.Web
{
    public class UserRegistrationEvent<C, R> : INotification where C : UserRegistrationResult where R : UserRegistrationRequest
    {
        private readonly C _result;
        private readonly R _request;

        public UserRegistrationEvent(C result, R request)
        {
            _result = result;
            _request = request;
        }
        public C Result { get { return _result; } }
        public R Request { get { return _request; } }

    }
}
