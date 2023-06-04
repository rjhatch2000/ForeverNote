using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Models.Common;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Customer
{
    public partial class CustomerAddressListModel : BaseForeverNoteModel
    {
        public CustomerAddressListModel()
        {
            Addresses = new List<AddressModel>();
        }

        public IList<AddressModel> Addresses { get; set; }
    }
}