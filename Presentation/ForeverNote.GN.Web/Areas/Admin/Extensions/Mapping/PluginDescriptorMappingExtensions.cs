using ForeverNote.Core.Plugins;
using ForeverNote.Web.Areas.Admin.Models.Plugins;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class PluginDescriptorMappingExtensions
    {
        public static PluginModel ToModel(this PluginDescriptor entity)
        {
            return entity.MapTo<PluginDescriptor, PluginModel>();
        }
    }
}