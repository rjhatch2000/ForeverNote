using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Common;
using MediatR;

namespace ForeverNote.Web.Commands.Models.Common
{
    public class ContactUsCommand : IRequest<ContactUsModel>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public Language Language { get; set; }
    }
}
