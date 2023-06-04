using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Tax
{
    public partial class TaxProviderModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.Configuration.Tax.Providers.Fields.FriendlyName")]
        
        public string FriendlyName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Tax.Providers.Fields.SystemName")]
        
        public string SystemName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Tax.Providers.Fields.IsPrimaryTaxProvider")]
        public bool IsPrimaryTaxProvider { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Tax.Providers.Fields.Configure")]
        public string ConfigurationUrl { get; set; }
    }
}