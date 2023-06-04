using ForeverNote.Core.Domain.Common;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Checkout
{
    public partial class CheckoutPickupPointModel : BaseForeverNoteModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Address Address { get; set; }

        public string PickupFee { get; set; }

    }
}