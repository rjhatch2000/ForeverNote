using ForeverNote.Core.Caching;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Services.Catalog;
using ForeverNote.Web.Features.Models.Products;
using ForeverNote.Web.Infrastructure.Cache;
using ForeverNote.Web.Models.Catalog;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Features.Handlers.Products
{
    public class GetProductReviewOverviewHandler : IRequestHandler<GetProductReviewOverview, ProductReviewOverviewModel>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IProductService _productService;
        private readonly CatalogSettings _catalogSettings;

        public GetProductReviewOverviewHandler(
            ICacheManager cacheManager, 
            IProductService productService, 
            CatalogSettings catalogSettings)
        {
            _cacheManager = cacheManager;
            _productService = productService;
            _catalogSettings = catalogSettings;
        }

        public async Task<ProductReviewOverviewModel> Handle(GetProductReviewOverview request, CancellationToken cancellationToken)
        {
            ProductReviewOverviewModel productReview = null;

            if (_catalogSettings.ShowProductReviewsPerStore)
            {
                string cacheKey = string.Format(ModelCacheEventConst.PRODUCT_REVIEWS_MODEL_KEY, request.Product.Id, request.Store.Id);

                productReview = await _cacheManager.GetAsync(cacheKey, async () =>
                {
                    return new ProductReviewOverviewModel {
                        RatingSum = await _productService.RatingSumProduct(request.Product.Id, _catalogSettings.ShowProductReviewsPerStore ? request.Store.Id : ""),
                        TotalReviews = await _productService.TotalReviewsProduct(request.Product.Id, _catalogSettings.ShowProductReviewsPerStore ? request.Store.Id : ""),
                    };
                });
            }
            else
            {
                productReview = new ProductReviewOverviewModel() {
                    RatingSum = request.Product.ApprovedRatingSum,
                    TotalReviews = request.Product.ApprovedTotalReviews
                };
            }

            if (productReview != null)
            {
                productReview.ProductId = request.Product.Id;
                productReview.AllowCustomerReviews = request.Product.AllowCustomerReviews;
            }
            return productReview;
        }
    }
}
