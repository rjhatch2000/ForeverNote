using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Blogs
{
    public partial class BlogPostTagModel : BaseForeverNoteModel
    {
        public string Name { get; set; }

        public int BlogPostCount { get; set; }
    }
}