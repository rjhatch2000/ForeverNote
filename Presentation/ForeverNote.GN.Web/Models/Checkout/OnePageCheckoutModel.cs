using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Checkout
{
    public partial class OnePageCheckoutModel : BaseForeverNoteModel
    {
        public bool ShippingRequired { get; set; }
        public bool DisableBillingAddressCheckoutStep { get; set; }
        public CheckoutBillingAddressModel BillingAddress { get; set; }
        public bool HasSinglePaymentMethod { get; set; }
    }
}