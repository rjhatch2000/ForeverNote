using ForeverNote.Core.Domain.Forums;
using ForeverNote.Web.Models.Boards;
using MediatR;

namespace ForeverNote.Web.Features.Models.Boards
{
    public class GetForumTopicRow : IRequest<ForumTopicRowModel>
    {
        public ForumTopic Topic { get; set; }
    }
}
