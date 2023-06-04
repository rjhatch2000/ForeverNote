using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Orders
{
    public partial class NeverSoldReportLineModel : BaseForeverNoteModel
    {
        public string ProductId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Reports.NeverSold.Fields.Name")]
        public string ProductName { get; set; }
    }
}