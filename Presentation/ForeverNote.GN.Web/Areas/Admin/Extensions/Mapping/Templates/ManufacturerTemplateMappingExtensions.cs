using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Web.Areas.Admin.Models.Templates;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class ManufacturerTemplateMappingExtensions
    {
        public static ManufacturerTemplateModel ToModel(this ManufacturerTemplate entity)
        {
            return entity.MapTo<ManufacturerTemplate, ManufacturerTemplateModel>();
        }

        public static ManufacturerTemplate ToEntity(this ManufacturerTemplateModel model)
        {
            return model.MapTo<ManufacturerTemplateModel, ManufacturerTemplate>();
        }

        public static ManufacturerTemplate ToEntity(this ManufacturerTemplateModel model, ManufacturerTemplate destination)
        {
            return model.MapTo(destination);
        }
    }
}