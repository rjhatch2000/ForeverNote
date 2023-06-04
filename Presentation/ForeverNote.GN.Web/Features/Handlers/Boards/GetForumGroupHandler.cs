using ForeverNote.Services.Forums;
using ForeverNote.Services.Seo;
using ForeverNote.Web.Extensions;
using ForeverNote.Web.Features.Models.Boards;
using ForeverNote.Web.Models.Boards;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Features.Handlers.Boards
{
    public class GetForumGroupHandler : IRequestHandler<GetForumGroup, ForumGroupModel>
    {
        private readonly IForumService _forumService;

        public GetForumGroupHandler(IForumService forumService)
        {
            _forumService = forumService;
        }

        public async Task<ForumGroupModel> Handle(GetForumGroup request, CancellationToken cancellationToken)
        {
            var forumGroupModel = new ForumGroupModel {
                Id = request.ForumGroup.Id,
                Name = request.ForumGroup.Name,
                SeName = request.ForumGroup.GetSeName(),
            };
            var forums = await _forumService.GetAllForumsByGroupId(request.ForumGroup.Id);
            foreach (var forum in forums)
            {
                forumGroupModel.Forums.Add(forum.ToModel());
            }
            return forumGroupModel;
        }
    }
}
