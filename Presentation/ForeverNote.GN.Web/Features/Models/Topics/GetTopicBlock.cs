using ForeverNote.Web.Models.Topics;
using MediatR;

namespace ForeverNote.Web.Features.Models.Topics
{
    public class GetTopicBlock : IRequest<TopicModel>
    {
        public string SystemName { get; set; }
        public string TopicId { get; set; }
    }
}
