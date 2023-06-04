
using ForeverNote.Web.Framework;
using ForeverNote.Web.Framework.Mvc;
using ForeverNote.Web.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ForeverNote.Web.Framework.Mvc.ModelBinding;

namespace ForeverNote.Web.Areas.Admin.Models.Catalog
{
    public partial class BulkEditProductModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.Catalog.BulkEdit.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.BulkEdit.Fields.SKU")]
        
        public string Sku { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.BulkEdit.Fields.Price")]
        public decimal Price { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.BulkEdit.Fields.OldPrice")]
        public decimal OldPrice { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.BulkEdit.Fields.ManageInventoryMethod")]
        public string ManageInventoryMethod { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.BulkEdit.Fields.StockQuantity")]
        public int StockQuantity { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.BulkEdit.Fields.Published")]
        public bool Published { get; set; }
    }
}