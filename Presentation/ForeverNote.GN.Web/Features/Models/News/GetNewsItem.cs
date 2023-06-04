using ForeverNote.Core.Domain.News;
using ForeverNote.Web.Models.News;
using MediatR;

namespace ForeverNote.Web.Features.Models.News
{
    public class GetNewsItem : IRequest<NewsItemModel>
    {
        public NewsItem NewsItem { get; set; }
    }
}
