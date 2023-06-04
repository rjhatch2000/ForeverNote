using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Models.Common;
using ForeverNote.Web.Areas.Admin.Validators.Customers;

namespace ForeverNote.Web.Areas.Admin.Models.Customers
{
    [Validator(typeof(CustomerAddressValidator))]
    public partial class CustomerAddressModel : BaseForeverNoteModel
    {
        public string CustomerId { get; set; }

        public AddressModel Address { get; set; }
    }
}