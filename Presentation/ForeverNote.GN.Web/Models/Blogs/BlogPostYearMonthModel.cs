using ForeverNote.Web.Framework.Mvc.Models;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Blogs
{
    public partial class BlogPostYearModel : BaseForeverNoteModel
    {
        public BlogPostYearModel()
        {
            Months = new List<BlogPostMonthModel>();
        }
        public int Year { get; set; }
        public IList<BlogPostMonthModel> Months { get; set; }
    }
    public partial class BlogPostMonthModel : BaseForeverNoteModel
    {
        public int Month { get; set; }

        public int BlogPostCount { get; set; }
    }
}