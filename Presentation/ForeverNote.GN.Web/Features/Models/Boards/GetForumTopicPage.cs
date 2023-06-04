using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Forums;
using ForeverNote.Web.Models.Boards;
using MediatR;

namespace ForeverNote.Web.Features.Models.Boards
{
    public class GetForumTopicPage : IRequest<ForumTopicPageModel>
    {
        public Customer Customer { get; set; }
        public ForumTopic ForumTopic { get; set; }
        public int PageNumber { get; set; }
    }
}
