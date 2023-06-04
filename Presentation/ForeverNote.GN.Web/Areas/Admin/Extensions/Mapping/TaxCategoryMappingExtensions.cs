using ForeverNote.Core.Domain.Tax;
using ForeverNote.Web.Areas.Admin.Models.Tax;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class TaxCategoryMappingExtensions
    {
        public static TaxCategoryModel ToModel(this TaxCategory entity)
        {
            return entity.MapTo<TaxCategory, TaxCategoryModel>();
        }

        public static TaxCategory ToEntity(this TaxCategoryModel model)
        {
            return model.MapTo<TaxCategoryModel, TaxCategory>();
        }

        public static TaxCategory ToEntity(this TaxCategoryModel model, TaxCategory destination)
        {
            return model.MapTo(destination);
        }
    }
}