using ForeverNote.Services.Cms;
using ForeverNote.Web.Areas.Admin.Models.Cms;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class IWidgetPluginMappingExtensions
    {
        public static WidgetModel ToModel(this IWidgetPlugin entity)
        {
            return entity.MapTo<IWidgetPlugin, WidgetModel>();
        }
    }
}