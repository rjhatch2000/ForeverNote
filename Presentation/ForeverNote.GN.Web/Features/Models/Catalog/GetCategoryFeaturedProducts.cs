using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Catalog;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Web.Features.Models.Catalog
{
    public class GetCategoryFeaturedProducts : IRequest<IList<CategoryModel>>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public Language Language { get; set; }

    }
}
