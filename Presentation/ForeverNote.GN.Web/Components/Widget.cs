using ForeverNote.Core;
using ForeverNote.Core.Caching;
using ForeverNote.Web.Framework.Components;
using ForeverNote.Web.Framework.Themes;
using ForeverNote.Services.Cms;
using ForeverNote.Web.Infrastructure.Cache;
using ForeverNote.Web.Models.Cms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Web.ViewComponents
{
    public class WidgetViewComponent : BaseViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly ICacheManager _cacheManager;
        private readonly IWidgetService _widgetService;
        private readonly IThemeContext _themeContext;

        public WidgetViewComponent(IStoreContext storeContext, ICacheManager cacheManager,
            IWidgetService widgetService, IThemeContext themeContext)
        {
            _storeContext = storeContext;
            _cacheManager = cacheManager;
            _widgetService = widgetService;
            _themeContext = themeContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData = null)
        {
            var cacheKey = string.Format(ModelCacheEventConst.WIDGET_MODEL_KEY,
                           _storeContext.CurrentStore.Id, widgetZone, _themeContext.WorkingThemeName);

            //add widget zone to view component arguments
            additionalData = new RouteValueDictionary(additionalData)
            {
                { "widgetZone", widgetZone },
                { "additionalData", additionalData}
            };

            var cachedModel = await _cacheManager.GetAsync(cacheKey, async () =>
            {
                //model
                var model = new List<RenderWidgetModel>();

                var widgets = _widgetService.LoadActiveWidgetsByWidgetZone(widgetZone, _storeContext.CurrentStore.Id);
                foreach (var widget in widgets)
                {
                    widget.GetPublicViewComponent(widgetZone, out string viewComponentName);

                    var widgetModel = new RenderWidgetModel {
                        WidgetViewComponentName = viewComponentName,
                        WidgetViewComponentArguments = additionalData
                    };

                    model.Add(widgetModel);
                }
                return await Task.FromResult(model);
            });

            //"WidgetViewComponentArguments" property of widget models depends on "additionalData".
            //We need to clone the cached model before modifications (the updated one should not be cached)
            var clonedModel = new List<RenderWidgetModel>();

            foreach (var widgetModel in cachedModel)
            {
                var clonedWidgetModel = new RenderWidgetModel {
                    WidgetViewComponentName = widgetModel.WidgetViewComponentName
                };

                if (widgetModel.WidgetViewComponentArguments != null)
                    clonedWidgetModel.WidgetViewComponentArguments = new RouteValueDictionary(widgetModel.WidgetViewComponentArguments);

                if (additionalData != null)
                {
                    if (clonedWidgetModel.WidgetViewComponentArguments == null)
                        clonedWidgetModel.WidgetViewComponentArguments = new RouteValueDictionary();

                    clonedWidgetModel.WidgetViewComponentArguments = additionalData;
                }

                clonedModel.Add(clonedWidgetModel);
            }

            if (!clonedModel.Any())
                return Content("");

            return View(clonedModel);
        }
    }
}