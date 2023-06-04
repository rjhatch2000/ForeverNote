using ForeverNote.Web.Models.Newsletter;
using MediatR;
using System;

namespace ForeverNote.Web.Commands.Models.Newsletter
{
    public class SubscriptionActivationCommand : IRequest<SubscriptionActivationModel>
    {
        public Guid Token { get; set; }
        public bool Active { get; set; }
    }
}
