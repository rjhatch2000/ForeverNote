﻿using ForeverNote.Core.Domain.Blogs;
using ForeverNote.Web.Framework.Components;
using ForeverNote.Web.Features.Models.Blogs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ForeverNote.Web.ViewComponents
{
    public class BlogMonthsViewComponent : BaseViewComponent
    {
        private readonly IMediator _mediator;
        private readonly BlogSettings _blogSettings;

        public BlogMonthsViewComponent(IMediator mediator, BlogSettings blogSettings)
        {
            _mediator = mediator;
            _blogSettings = blogSettings;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_blogSettings.Enabled)
                return Content("");

            var model = await _mediator.Send(new GetBlogPostYear());
            return View(model);
        }
    }
}