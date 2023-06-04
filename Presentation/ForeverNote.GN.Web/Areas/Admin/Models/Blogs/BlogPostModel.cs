using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mapping;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Blogs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Blogs
{
    [Validator(typeof(BlogPostValidator))]
    public partial class BlogPostModel : BaseForeverNoteEntityModel, ILocalizedModel<BlogLocalizedModel>, IStoreMappingModel
    {
        public BlogPostModel()
        {
            this.AvailableStores = new List<StoreModel>();
            Locales = new List<BlogLocalizedModel>();
        }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.Title")]
        public string Title { get; set; }

        [UIHint("Picture")]
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.Picture")]
        public string PictureId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.Body")]
        
        public string Body { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.BodyOverview")]
        
        public string BodyOverview { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.AllowComments")]
        public bool AllowComments { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.Tags")]
        
        public string Tags { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.Comments")]
        public int Comments { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.StartDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartDate { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.EndDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDate { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.MetaKeywords")]
        
        public string MetaKeywords { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.MetaDescription")]
        
        public string MetaDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.MetaTitle")]
        
        public string MetaTitle { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.SeName")]
        
        public string SeName { get; set; }

        public IList<BlogLocalizedModel> Locales { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        //Store mapping
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }


    }

    public partial class BlogLocalizedModel : ILocalizedModelLocal, ISlugModelLocal
    {
        public string LanguageId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.Title")]
        public string Title { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.BodyOverview")]
        
        public string BodyOverview { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.Body")]
        
        public string Body { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.MetaKeywords")]
        
        public string MetaKeywords { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.MetaDescription")]
        
        public string MetaDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.MetaTitle")]
        
        public string MetaTitle { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.SeName")]
        
        public string SeName { get; set; }

    }
}