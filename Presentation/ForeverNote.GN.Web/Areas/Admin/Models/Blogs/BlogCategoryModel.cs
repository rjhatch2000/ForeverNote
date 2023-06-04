using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mapping;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Blogs;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Blogs
{
    [Validator(typeof(BlogCategoryValidator))]
    public partial class BlogCategoryModel : BaseForeverNoteEntityModel, ILocalizedModel<BlogCategoryLocalizedModel>, IStoreMappingModel
    {
        public BlogCategoryModel()
        {
            this.AvailableStores = new List<StoreModel>();
            Locales = new List<BlogCategoryLocalizedModel>();
        }
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogCategory.Fields.Name")]
        public string Name { get; set; }
        
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogCategory.Fields.SeName")]
        public string SeName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogCategory.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<BlogCategoryLocalizedModel> Locales { get; set; }
        //Store mapping
        public bool LimitedToStores { get; set; }
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }
    }
    public partial class BlogCategoryLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogCategory.Fields.Name")]
        public string Name { get; set; }
    }

    public partial class AddBlogPostCategoryModel : BaseForeverNoteModel
    {
        public AddBlogPostCategoryModel()
        {
            AvailableStores = new List<SelectListItem>();
        }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogCategory.SearchBlogTitle")]

        public string SearchBlogTitle { get; set; }
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogCategory.SearchStore")]
        public string SearchStoreId { get; set; }
        
        public IList<SelectListItem> AvailableStores { get; set; }

        public string CategoryId { get; set; }

        public string[] SelectedBlogPostIds { get; set; }
    }
}
