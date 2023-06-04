using ForeverNote.Services.Shipping;
using ForeverNote.Web.Areas.Admin.Models.Shipping;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class IShippingRateComputationMethodMappingExtensions
    {
        public static ShippingRateComputationMethodModel ToModel(this IShippingRateComputationMethod entity)
        {
            return entity.MapTo<IShippingRateComputationMethod, ShippingRateComputationMethodModel>();
        }
    }
}