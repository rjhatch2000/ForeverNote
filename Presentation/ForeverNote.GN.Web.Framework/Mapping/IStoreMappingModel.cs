using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using System.Collections.Generic;

namespace ForeverNote.Web.Framework.Mapping
{
    public interface IStoreMappingModel
    {
        [ForeverNoteResourceDisplayName("Admin.Catalog.Categories.Fields.LimitedToStores")]
        bool LimitedToStores { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Categories.Fields.AvailableStores")]
        List<StoreModel> AvailableStores { get; set; }
        string[] SelectedStoreIds { get; set; }
    }
}
