using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Blogs
{
    public class BlogCategoryPost : BaseForeverNoteEntityModel
    {
        public string BlogPostId { get; set; }
        public string Name { get; set; }
    }
}
