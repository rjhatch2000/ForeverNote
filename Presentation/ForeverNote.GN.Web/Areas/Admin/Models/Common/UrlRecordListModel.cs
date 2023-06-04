using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Common
{
    public partial class UrlRecordListModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.System.SeNames.Name")]
        
        public string SeName { get; set; }
    }
}