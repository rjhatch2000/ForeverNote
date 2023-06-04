using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Models.Common;
using ForeverNote.Web.Validators.Customer;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Checkout
{
    [Validator(typeof(CheckoutBillingAddressValidator))]
    public partial class CheckoutBillingAddressModel : BaseForeverNoteModel
    {
        public CheckoutBillingAddressModel()
        {
            ExistingAddresses = new List<AddressModel>();
            NewAddress = new AddressModel();
        }

        public IList<AddressModel> ExistingAddresses { get; set; }

        public AddressModel NewAddress { get; set; }

        public bool ShipToSameAddress { get; set; }
        public bool ShipToSameAddressAllowed { get; set; }
        /// <summary>
        /// Used on one-page checkout page
        /// </summary>
        public bool NewAddressPreselected { get; set; }
    }
}