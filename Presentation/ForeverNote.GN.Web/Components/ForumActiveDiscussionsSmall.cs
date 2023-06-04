using ForeverNote.Web.Framework.Components;
using ForeverNote.Web.Features.Models.Boards;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ForeverNote.Web.ViewComponents
{
    public class ForumActiveDiscussionsSmallViewComponent : BaseViewComponent
    {
        private readonly IMediator _mediator;

        public ForumActiveDiscussionsSmallViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _mediator.Send(new GetActiveDiscussions());
            if (model == null)
                return Content("");

            return View(model);

        }
    }
}