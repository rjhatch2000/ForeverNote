using ForeverNote.Web.Models.Newsletter;
using MediatR;

namespace ForeverNote.Web.Commands.Models.Newsletter
{
    public class SubscribeNewsletterCommand : IRequest<SubscribeNewsletterResultModel>
    {
        public string Email { get; set; }
        public bool Subscribe { get; set; }
    }
}
