﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ForeverNote.Core.Caching;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Customers;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Seo;
using ForeverNote.Web.Features.Models.Catalog;
using ForeverNote.Web.Infrastructure.Cache;
using ForeverNote.Web.Models.Catalog;
using MediatR;

namespace ForeverNote.Web.Features.Handlers.Catalog
{
    public class GetManufacturerNavigationHandler : IRequestHandler<GetManufacturerNavigation, ManufacturerNavigationModel>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IManufacturerService _manufacturerService;
        private readonly CatalogSettings _catalogSettings;

        public GetManufacturerNavigationHandler(ICacheManager cacheManager, 
            IManufacturerService manufacturerService, 
            CatalogSettings catalogSettings)
        {
            _cacheManager = cacheManager;
            _manufacturerService = manufacturerService;
            _catalogSettings = catalogSettings;
        }

        public async Task<ManufacturerNavigationModel> Handle(GetManufacturerNavigation request, CancellationToken cancellationToken)
        {
            string cacheKey = string.Format(ModelCacheEventConst.MANUFACTURER_NAVIGATION_MODEL_KEY,
                request.CurrentManufacturerId, request.Language.Id, string.Join(",", request.Customer.GetCustomerRoleIds()),
                request.Store.Id);
            var cacheModel = await _cacheManager.GetAsync(cacheKey, async () =>
            {
                var currentManufacturer = await _manufacturerService.GetManufacturerById(request.CurrentManufacturerId);
                var manufacturers = await _manufacturerService.GetAllManufacturers(pageSize: _catalogSettings.ManufacturersBlockItemsToDisplay, storeId: request.Store.Id);
                var model = new ManufacturerNavigationModel {
                    TotalManufacturers = manufacturers.TotalCount
                };

                foreach (var manufacturer in manufacturers)
                {
                    var modelMan = new ManufacturerBriefInfoModel {
                        Id = manufacturer.Id,
                        Name = manufacturer.GetLocalized(x => x.Name, request.Language.Id),
                        Icon = manufacturer.Icon,
                        SeName = manufacturer.GetSeName(request.Language.Id),
                        IsActive = currentManufacturer != null && currentManufacturer.Id == manufacturer.Id,
                    };
                    model.Manufacturers.Add(modelMan);
                }
                return model;
            });
            return cacheModel;
        }
    }
}
