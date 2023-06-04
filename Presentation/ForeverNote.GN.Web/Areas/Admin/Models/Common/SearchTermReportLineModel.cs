using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Common
{
    public partial class SearchTermReportLineModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.SearchTermReport.Keyword")]
        public string Keyword { get; set; }

        [ForeverNoteResourceDisplayName("Admin.SearchTermReport.Count")]
        public int Count { get; set; }
    }
}
