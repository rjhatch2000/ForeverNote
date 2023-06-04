using MediatR;

namespace ForeverNote.Web.Features.Models.Topics
{
    public class GetTopicTemplateViewPath : IRequest<string>
    {
        public string TemplateId { get; set; }
    }
}
