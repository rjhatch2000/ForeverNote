using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Models.Catalog;
using ForeverNote.Web.Models.Topics;
using ForeverNote.Web.Models.Blogs;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Common
{
    public partial class SitemapModel : BaseForeverNoteModel
    {
        public SitemapModel()
        {
            Products = new List<ProductOverviewModel>();
            Categories = new List<CategoryModel>();
            Manufacturers = new List<ManufacturerModel>();
            Topics = new List<TopicModel>();
            BlogPosts = new List<BlogPostModel>();
        }
        public IList<ProductOverviewModel> Products { get; set; }
        public IList<CategoryModel> Categories { get; set; }
        public IList<ManufacturerModel> Manufacturers { get; set; }
        public IList<TopicModel> Topics { get; set; }
        public IList<BlogPostModel> BlogPosts { get; set; }

        public bool NewsEnabled { get; set; }
        public bool BlogEnabled { get; set; }
        public bool ForumEnabled { get; set; }
        public bool KnowledgebaseEnabled { get; set; }
    }
}