using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Checkout
{
    public partial class CheckoutProgressModel : BaseForeverNoteModel
    {
        public CheckoutProgressStep CheckoutProgressStep { get; set; }
    }

    public enum CheckoutProgressStep
    {
        Cart,
        Address,
        Shipping,
        Payment,
        Confirm,
        Complete
    }
}