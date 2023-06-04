using ForeverNote.Web.Models.Common;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Profile
{
    public partial class ProfilePostsModel
    {
        public IList<PostsModel> Posts { get; set; }
        public PagerModel PagerModel { get; set; }
    }
}