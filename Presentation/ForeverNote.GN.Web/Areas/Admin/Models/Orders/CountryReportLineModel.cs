using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Orders
{
    public partial class CountryReportLineModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.Reports.Country.Fields.CountryName")]
        public string CountryName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Reports.Country.Fields.TotalOrders")]
        public int TotalOrders { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Reports.Country.Fields.SumOrders")]
        public string SumOrders { get; set; }
    }
}