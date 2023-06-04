using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Orders
{
    public partial class OrderIncompleteReportLineModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.Reports.Incomplete.Item")]
        public string Item { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Reports.Incomplete.Total")]
        public string Total { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Reports.Incomplete.Count")]
        public int Count { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Reports.Incomplete.View")]
        public string ViewLink { get; set; }
    }
}
