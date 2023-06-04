using ForeverNote.Core.Domain.Vendors;
using ForeverNote.Web.Areas.Admin.Models.Settings;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class VendorSettingsMappingExtensions
    {
        public static VendorSettingsModel ToModel(this VendorSettings entity)
        {
            return entity.MapTo<VendorSettings, VendorSettingsModel>();
        }
        public static VendorSettings ToEntity(this VendorSettingsModel model, VendorSettings destination)
        {
            return model.MapTo(destination);
        }
    }
}