using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Validators.Customer;

namespace ForeverNote.Web.Models.Orders
{
    [Validator(typeof(AddOrderNoteValidator))]
    public class AddOrderNoteModel : BaseForeverNoteModel
    {
        public string OrderId { get; set; }
        public string Note { get; set; }
    }
}
