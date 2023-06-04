using ForeverNote.Services.Tax;
using ForeverNote.Web.Areas.Admin.Models.Tax;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class ITaxProviderMappingExtensions
    {
        public static TaxProviderModel ToModel(this ITaxProvider entity)
        {
            return entity.MapTo<ITaxProvider, TaxProviderModel>();
        }
    }
}