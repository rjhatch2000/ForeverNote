using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Web.Models.Customer;
using MediatR;

namespace ForeverNote.Web.Features.Models.Customers
{
    public class GetDocuments : IRequest<DocumentsModel>
    {
        public Customer Customer { get; set; }
        public Language Language { get; set; }
    }
}
