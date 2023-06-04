using ForeverNote.Web.Models.News;
using MediatR;

namespace ForeverNote.Web.Features.Models.News
{
    public class GetNewsItemList : IRequest<NewsItemListModel>
    {
        public NewsPagingFilteringModel Command { get; set; }
    }
}
