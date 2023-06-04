using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Blogs
{
    public partial class BlogPostCategoryModel : BaseForeverNoteModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SeName { get; set; }
        public int BlogPostCount { get; set; }
    }
}
