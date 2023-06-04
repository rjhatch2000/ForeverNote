using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Models.Common;

namespace ForeverNote.Web.Areas.Admin.Models.Customers
{
    public partial class CustomerReviewModel
    {
        public string CustomerId { get; set; }

        public ReviewModel Review { get; set; }
    }
}