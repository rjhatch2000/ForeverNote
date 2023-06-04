using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using System;

namespace ForeverNote.Web.Areas.Admin.Models.Blogs
{
    public partial class BlogCommentModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.Comments.Fields.BlogPost")]
        public string BlogPostId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.Comments.Fields.BlogPost")]
        
        public string BlogPostTitle { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.Comments.Fields.Customer")]
        public string CustomerId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.Comments.Fields.Customer")]
        public string CustomerInfo { get; set; }

        
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.Comments.Fields.Comment")]
        public string Comment { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.Comments.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

    }
}