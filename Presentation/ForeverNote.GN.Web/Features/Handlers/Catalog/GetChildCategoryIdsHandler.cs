using ForeverNote.Core.Caching;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Customers;
using ForeverNote.Web.Features.Models.Catalog;
using ForeverNote.Web.Infrastructure.Cache;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Features.Handlers.Catalog
{
    public class GetChildCategoryIdsHandler : IRequestHandler<GetChildCategoryIds, IList<string>>
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICategoryService _categoryService;

        public GetChildCategoryIdsHandler(ICacheManager cacheManager, ICategoryService categoryService)
        {
            _cacheManager = cacheManager;
            _categoryService = categoryService;
        }

        public async Task<IList<string>> Handle(GetChildCategoryIds request, CancellationToken cancellationToken)
        {
            return await GetChildCategoryIds(request);
        }

        private async Task<List<string>> GetChildCategoryIds(GetChildCategoryIds request)
        {
            string cacheKey = string.Format(ModelCacheEventConst.CATEGORY_CHILD_IDENTIFIERS_MODEL_KEY,
                request.ParentCategoryId,
                string.Join(",", request.Customer.GetCustomerRoleIds()),
                request.Store.Id);
            return await _cacheManager.GetAsync(cacheKey, async () =>
            {
                var categoriesIds = new List<string>();
                var categories = await _categoryService.GetAllCategoriesByParentCategoryId(request.ParentCategoryId);
                foreach (var category in categories)
                {
                    categoriesIds.Add(category.Id);
                    request.ParentCategoryId = category.Id;
                    categoriesIds.AddRange(await GetChildCategoryIds(request));
                }
                return categoriesIds;
            });
        }
    }
}
