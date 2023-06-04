using ForeverNote.Web.Framework.Components;
using ForeverNote.Web.Features.Models.Boards;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ForeverNote.Web.ViewComponents
{
    public class ForumLastPostViewComponent : BaseViewComponent
    {
        private readonly IMediator _mediator;
        public ForumLastPostViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(string forumPostId, bool showTopic)
        {
            var model = await _mediator.Send(new GetLastPost() { ForumPostId = forumPostId, ShowTopic = showTopic });
            return View(model);
        }
    }
}