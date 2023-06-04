using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Models.Common;
using ForeverNote.Web.Validators.Customer;

namespace ForeverNote.Web.Models.Customer
{
    [Validator(typeof(CustomerAddressEditValidator))]
    public partial class CustomerAddressEditModel : BaseForeverNoteModel
    {
        public CustomerAddressEditModel()
        {
            this.Address = new AddressModel();
        }
        public AddressModel Address { get; set; }
    }
}