using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Catalog;
using MediatR;

namespace ForeverNote.Web.Features.Models.Catalog
{
    public class GetSearchBox : IRequest<SearchBoxModel>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
    }
}
