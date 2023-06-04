using ForeverNote.Core.Domain.Customers;
using ForeverNote.Web.Features.Models.Newsletter;
using ForeverNote.Web.Models.Newsletter;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Features.Handlers.Newsletter
{
    public class GetNewsletterBoxHandler : IRequestHandler<GetNewsletterBox, NewsletterBoxModel>
    {
        private readonly CustomerSettings _customerSettings;

        public GetNewsletterBoxHandler(CustomerSettings customerSettings)
        {
            _customerSettings = customerSettings;
        }

        public async Task<NewsletterBoxModel> Handle(GetNewsletterBox request, CancellationToken cancellationToken)
        {
            if (_customerSettings.HideNewsletterBlock)
                return null;

            var model = new NewsletterBoxModel() {
                AllowToUnsubscribe = _customerSettings.NewsletterBlockAllowToUnsubscribe
            };

            return await Task.FromResult(model);
        }
    }
}
