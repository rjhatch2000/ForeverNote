using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Web.Models.Catalog;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Web.Features.Models.Products
{
    public class GetProductSpecification : IRequest<IList<ProductSpecificationModel>>
    {
        public Product Product { get; set; }
        public Language Language { get; set; }

    }
}
