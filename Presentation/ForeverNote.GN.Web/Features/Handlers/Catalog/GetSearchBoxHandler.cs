﻿using ForeverNote.Core.Caching;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Customers;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Features.Models.Catalog;
using ForeverNote.Web.Infrastructure.Cache;
using ForeverNote.Web.Models.Catalog;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Features.Handlers.Catalog
{
    public class GetSearchBoxHandler : IRequestHandler<GetSearchBox, SearchBoxModel>
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICategoryService _categoryService;
        private readonly ILocalizationService _localizationService;
        private readonly CatalogSettings _catalogSettings;

        public GetSearchBoxHandler(
            ICacheManager cacheManager,
            ICategoryService categoryService,
            ILocalizationService localizationService,
            CatalogSettings catalogSettings)
        {
            _cacheManager = cacheManager;
            _categoryService = categoryService;
            _localizationService = localizationService;
            _catalogSettings = catalogSettings;
        }

        public async Task<SearchBoxModel> Handle(GetSearchBox request, CancellationToken cancellationToken)
        {
            string cacheKey = string.Format(ModelCacheEventConst.CATEGORY_ALL_SEARCHBOX,
                string.Join(",", request.Customer.GetCustomerRoleIds()),
                request.Store.Id);

            return await _cacheManager.GetAsync(cacheKey, async () =>
            {
                var searchbocategories = await _categoryService.GetAllCategoriesSearchBox();

                var availableCategories = new List<SelectListItem>();
                if (searchbocategories.Any())
                {
                    availableCategories.Add(new SelectListItem { Text = _localizationService.GetResource("Common.All"), Value = "" });
                    foreach (var s in searchbocategories)
                        availableCategories.Add(new SelectListItem { Text = s.Name, Value = s.Id.ToString() });
                }

                var model = new SearchBoxModel {
                    AutoCompleteEnabled = _catalogSettings.ProductSearchAutoCompleteEnabled,
                    ShowProductImagesInSearchAutoComplete = _catalogSettings.ShowProductImagesInSearchAutoComplete,
                    SearchTermMinimumLength = _catalogSettings.ProductSearchTermMinimumLength,
                    AvailableCategories = availableCategories
                };

                return model;
            });

        }
    }
}
