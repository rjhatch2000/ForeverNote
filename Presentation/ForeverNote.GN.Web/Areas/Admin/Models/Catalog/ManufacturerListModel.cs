using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Catalog
{
    public partial class ManufacturerListModel : BaseForeverNoteModel
    {
        public ManufacturerListModel()
        {
            AvailableStores = new List<SelectListItem>();
        }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Manufacturers.List.SearchManufacturerName")]
        
        public string SearchManufacturerName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Manufacturers.List.SearchStore")]
        public string SearchStoreId { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
    }
}