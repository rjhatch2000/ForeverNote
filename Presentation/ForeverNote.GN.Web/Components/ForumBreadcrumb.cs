using ForeverNote.Web.Framework.Components;
using ForeverNote.Web.Features.Models.Boards;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ForeverNote.Web.ViewComponents
{
    public class ForumBreadcrumbViewComponent : BaseViewComponent
    {
        private readonly IMediator _mediator;
        public ForumBreadcrumbViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(string forumGroupId, string forumId, string forumTopicId)
        {
            var model = await _mediator.Send(new GetForumBreadcrumb() {
                ForumGroupId = forumGroupId,
                ForumId = forumId,
                ForumTopicId = forumTopicId,
            });
            return View(model);
        }
    }
}