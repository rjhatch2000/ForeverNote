using ForeverNote.Web.Framework.Mvc.Models;
using System;

namespace ForeverNote.Web.Models.Blogs
{
    public partial class BlogCommentModel : BaseForeverNoteEntityModel
    {
        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAvatarUrl { get; set; }

        public string CommentText { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool AllowViewingProfiles { get; set; }
    }
}