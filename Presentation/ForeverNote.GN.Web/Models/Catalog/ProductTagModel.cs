using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Catalog
{
    public partial class ProductTagModel : BaseForeverNoteEntityModel
    {
        public string Name { get; set; }

        public string SeName { get; set; }

        public int ProductCount { get; set; }
    }
}