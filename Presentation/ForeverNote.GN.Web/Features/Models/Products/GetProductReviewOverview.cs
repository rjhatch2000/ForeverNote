using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Catalog;
using MediatR;

namespace ForeverNote.Web.Features.Models.Products
{
    public class GetProductReviewOverview : IRequest<ProductReviewOverviewModel>
    {
        public Product Product { get; set; }
        public Language Language { get; set; }
        public Store Store { get; set; }
    }
}
