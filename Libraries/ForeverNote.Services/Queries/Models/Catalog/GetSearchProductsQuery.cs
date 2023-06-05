using ForeverNote.Core;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Customers;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Services.Queries.Models.Catalog
{
    public class GetSearchProductsQuery : IRequest<(IPagedList<Product> products, IList<string> filterableSpecificationAttributeOptionIds)>
    {
        public Customer Customer { get; set; }

        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = int.MaxValue;
        public IList<string> CategoryIds { get; set; } = null;
        public bool MarkedAsNewOnly { get; set; } = false;
        public bool? FeaturedProducts { get; set; } = null;
        public string ProductTag { get; set; } = "";
        public string Keywords { get; set; } = null;
        public bool SearchDescriptions { get; set; } = false;
        public bool SearchProductTags { get; set; } = false;
        public string LanguageId { get; set; } = "";
        public ProductSortingEnum OrderBy { get; set; } = ProductSortingEnum.Position;
    }
}
