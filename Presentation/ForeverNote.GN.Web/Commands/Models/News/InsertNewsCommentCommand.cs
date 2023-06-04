using ForeverNote.Core.Domain.News;
using ForeverNote.Web.Models.News;
using MediatR;

namespace ForeverNote.Web.Commands.Models.News
{
    public class InsertNewsCommentCommand : IRequest<NewsComment>
    {
        public NewsItem NewsItem { get; set; }
        public NewsItemModel Model { get; set; }
    }
}
