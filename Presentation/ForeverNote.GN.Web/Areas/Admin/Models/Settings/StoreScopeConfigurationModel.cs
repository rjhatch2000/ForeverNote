using ForeverNote.Web.Framework.Mvc.Models;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Settings
{
    public partial class StoreScopeConfigurationModel : BaseForeverNoteModel
    {
        public StoreScopeConfigurationModel()
        {
            Stores = new List<StoreModel>();
        }

        public string StoreId { get; set; }
        public IList<StoreModel> Stores { get; set; }
    }
}