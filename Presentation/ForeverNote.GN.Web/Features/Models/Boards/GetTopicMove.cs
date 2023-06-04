using ForeverNote.Core.Domain.Forums;
using ForeverNote.Web.Models.Boards;
using MediatR;

namespace ForeverNote.Web.Features.Models.Boards
{
    public class GetTopicMove : IRequest<TopicMoveModel>
    {
        public ForumTopic ForumTopic { get; set; }
    }
}
