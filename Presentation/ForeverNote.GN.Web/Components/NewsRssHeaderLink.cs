using ForeverNote.Core.Domain.News;
using ForeverNote.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace ForeverNote.Web.ViewComponents
{
    public class NewsRssHeaderLinkViewComponent : BaseViewComponent
    {
        private readonly NewsSettings _newsSettings;
        public NewsRssHeaderLinkViewComponent(NewsSettings newsSettings)
        {
            _newsSettings = newsSettings;
        }

        public IViewComponentResult Invoke()
        {
            if (!_newsSettings.Enabled || !_newsSettings.ShowHeaderRssUrl)
                return Content("");

            return View();

        }
    }
}