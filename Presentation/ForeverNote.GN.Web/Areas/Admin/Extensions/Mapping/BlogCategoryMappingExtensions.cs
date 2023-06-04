using ForeverNote.Core.Domain.Blogs;
using ForeverNote.Web.Areas.Admin.Models.Blogs;

namespace ForeverNote.Web.Areas.Admin.Extensions
{
    public static class BlogCategoryMappingExtensions
    {
        public static BlogCategoryModel ToModel(this BlogCategory entity)
        {
            return entity.MapTo<BlogCategory, BlogCategoryModel>();
        }

        public static BlogCategory ToEntity(this BlogCategoryModel model)
        {
            return model.MapTo<BlogCategoryModel, BlogCategory>();
        }

        public static BlogCategory ToEntity(this BlogCategoryModel model, BlogCategory destination)
        {
            return model.MapTo(destination);
        }
    }
}