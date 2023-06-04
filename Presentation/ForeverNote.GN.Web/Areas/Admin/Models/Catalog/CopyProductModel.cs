using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Catalog
{
    public partial class CopyProductModel : BaseForeverNoteEntityModel
    {

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Copy.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Copy.CopyImages")]
        public bool CopyImages { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Copy.Published")]
        public bool Published { get; set; }

    }
}