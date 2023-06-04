using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Orders
{
    public partial class BestsellersReportLineModel : BaseForeverNoteModel
    {
        public string ProductId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Reports.Bestsellers.Fields.Name")]
        public string ProductName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Reports.Bestsellers.Fields.TotalAmount")]
        public string TotalAmount { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Reports.Bestsellers.Fields.TotalQuantity")]
        public decimal TotalQuantity { get; set; }
    }
}