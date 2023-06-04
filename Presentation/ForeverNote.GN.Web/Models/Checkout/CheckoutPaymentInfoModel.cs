using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Checkout
{
    public partial class CheckoutPaymentInfoModel : BaseForeverNoteModel
    {
        public string PaymentViewComponentName { get; set; }

        /// <summary>
        /// Used on one-page checkout page
        /// </summary>
        public bool DisplayOrderTotals { get; set; }
    }
}