using ForeverNote.Core.Domain.Payments;
using ForeverNote.Services.Payments;
using MediatR;

namespace ForeverNote.Services.Events.Web
{
    public class VoidOrderDetailsEvent<R, C> : INotification where R : VoidPaymentResult where C : VoidPaymentRequest
    {
        private readonly R _result;
        private readonly C _containter;

        public VoidOrderDetailsEvent(R result, C containter)
        {
            _result = result;
            _containter = containter;
        }
        public R Result { get { return _result; } }
        public C Containter { get { return _containter; } }

    }

}
