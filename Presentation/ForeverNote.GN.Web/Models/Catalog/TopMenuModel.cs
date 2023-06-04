using ForeverNote.Web.Framework.Mvc.Models;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Catalog
{
    public partial class TopMenuModel : BaseForeverNoteModel
    {
        public TopMenuModel()
        {
            Categories = new List<CategorySimpleModel>();
            Topics = new List<TopMenuTopicModel>();
            Manufacturers = new List<TopMenuManufacturerModel> ();
        }

        public IList<CategorySimpleModel> Categories { get; set; }
        public IList<TopMenuTopicModel> Topics { get; set; }
        public IList<TopMenuManufacturerModel> Manufacturers { get; set; }

        public bool BlogEnabled { get; set; }
        public bool NewProductsEnabled { get; set; }
        public bool ForumEnabled { get; set; }

        public bool DisplayHomePageMenu { get; set; }
        public bool DisplayNewProductsMenu { get; set; }
        public bool DisplaySearchMenu { get; set; }
        public bool DisplayCustomerMenu { get; set; }
        public bool DisplayBlogMenu { get; set; }
        public bool DisplayForumsMenu { get; set; }
        public bool DisplayContactUsMenu { get; set; }

        #region Nested classes

        public class TopMenuTopicModel : BaseForeverNoteEntityModel
        {
            public string Name { get; set; }
            public string SeName { get; set; }
        }

        public class TopMenuManufacturerModel : BaseForeverNoteEntityModel
        {
            public string Name { get; set; }
            public string Icon { get; set; }
            public string SeName { get; set; }
        }


        public class CategoryLineModel : BaseForeverNoteModel
        {
            public int Level { get; set; }
            public bool ResponsiveMobileMenu { get; set; }
            public CategorySimpleModel Category { get; set; }
        }

        #endregion
    }
}