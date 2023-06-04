using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Checkout
{
    public partial class CheckoutCompletedModel : BaseForeverNoteModel
    {
        public string OrderId { get; set; }
        public int OrderNumber { get; set; }
        public string OrderCode { get; set; }
        public bool OnePageCheckoutEnabled { get; set; }
    }
}