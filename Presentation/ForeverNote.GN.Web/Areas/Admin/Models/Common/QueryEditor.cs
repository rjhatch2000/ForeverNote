
using ForeverNote.Web.Framework.Mvc.ModelBinding;

namespace ForeverNote.Web.Areas.Admin.Models.Common
{
    public partial class QueryEditor
    {
        [ForeverNoteResourceDisplayName("Admin.System.Field.QueryEditor")]
        public string Query { get; set; }
    }
}
