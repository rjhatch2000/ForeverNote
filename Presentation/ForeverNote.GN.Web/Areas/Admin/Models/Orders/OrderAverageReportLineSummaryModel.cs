using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Orders
{
    public partial class OrderAverageReportLineSummaryModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.Reports.Average.OrderStatus")]
        public string OrderStatus { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Reports.Average.SumTodayOrders")]
        public string SumTodayOrders { get; set; }
        
        [ForeverNoteResourceDisplayName("Admin.Reports.Average.SumThisWeekOrders")]
        public string SumThisWeekOrders { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Reports.Average.SumThisMonthOrders")]
        public string SumThisMonthOrders { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Reports.Average.SumThisYearOrders")]
        public string SumThisYearOrders { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Reports.Average.SumAllTimeOrders")]
        public string SumAllTimeOrders { get; set; }
    }
}
