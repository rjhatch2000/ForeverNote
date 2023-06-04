using ForeverNote.Core.Domain.Forums;
using ForeverNote.Web.Models.Boards;
using MediatR;

namespace ForeverNote.Web.Features.Models.Boards
{
    public class GetForumGroup : IRequest<ForumGroupModel>
    {
        public ForumGroup ForumGroup { get; set; }
    }
}
