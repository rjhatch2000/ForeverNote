﻿using ForeverNote.Core.Domain.Blogs;
using ForeverNote.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace ForeverNote.Web.ViewComponents
{
    public class BlogRssHeaderLinkViewComponent : BaseViewComponent
    {
        private readonly BlogSettings _blogSettings;
        public BlogRssHeaderLinkViewComponent(BlogSettings blogSettings)
        {
            _blogSettings = blogSettings;
        }

        public IViewComponentResult Invoke()
        {
            if (!_blogSettings.Enabled || !_blogSettings.ShowHeaderRssUrl)
                return Content("");

            return View();

        }
    }
}