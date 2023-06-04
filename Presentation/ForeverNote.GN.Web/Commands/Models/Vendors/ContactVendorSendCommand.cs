using ForeverNote.Core.Domain.Stores;
using ForeverNote.Core.Domain.Vendors;
using ForeverNote.Web.Models.Common;
using MediatR;

namespace ForeverNote.Web.Commands.Models.Vendors
{
    public class ContactVendorSendCommand : IRequest<ContactVendorModel>
    {
        public Vendor Vendor { get; set; }
        public Store Store { get; set; }
        public ContactVendorModel Model { get; set; }

    }
}
