using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;


namespace ForeverNote.Web.Areas.Admin.Models.Orders
{
    public partial class OrderPeriodReportLineModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.Reports.Period.Name")]
        public string Period { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Reports.Period.Count")]
        public int Count { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Reports.Period.Amount")]
        public decimal Amount { get; set; }

    }
}