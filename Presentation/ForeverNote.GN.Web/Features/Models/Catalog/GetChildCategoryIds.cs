using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Stores;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Web.Features.Models.Catalog
{
    public class GetChildCategoryIds : IRequest<IList<string>>
    {
        public string ParentCategoryId { get; set; }
        public Customer Customer { get; set; }
        public Store Store { get; set; }

    }
}
