﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ForeverNote.Core.Caching;
using ForeverNote.Core.Domain.Blogs;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Forums;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Seo;
using ForeverNote.Services.Topics;
using ForeverNote.Web.Features.Models.Catalog;
using ForeverNote.Web.Infrastructure.Cache;
using ForeverNote.Web.Models.Catalog;
using MediatR;

namespace ForeverNote.Web.Features.Handlers.Catalog
{
    public class GetTopMenuHandler : IRequestHandler<GetTopMenu, TopMenuModel>
    {
        private readonly IMediator _mediator;
        private readonly ITopicService _topicService;
        private readonly ICacheManager _cacheManager;
        private readonly IManufacturerService _manufacturerService;
        private readonly CatalogSettings _catalogSettings;
        private readonly BlogSettings _blogSettings;
        private readonly ForumSettings _forumSettings;
        private readonly MenuItemSettings _menuItemSettings;

        public GetTopMenuHandler(
            IMediator mediator, 
            ITopicService topicService, 
            ICacheManager cacheManager, 
            IManufacturerService manufacturerService, 
            CatalogSettings catalogSettings, 
            BlogSettings blogSettings, 
            ForumSettings forumSettings, 
            MenuItemSettings menuItemSettings)
        {
            _mediator = mediator;
            _topicService = topicService;
            _cacheManager = cacheManager;
            _manufacturerService = manufacturerService;
            _catalogSettings = catalogSettings;
            _blogSettings = blogSettings;
            _forumSettings = forumSettings;
            _menuItemSettings = menuItemSettings;
        }

        public async Task<TopMenuModel> Handle(GetTopMenu request, CancellationToken cancellationToken)
        {
            //categories
            var cachedCategoriesModel = await _mediator.Send(new GetCategorySimple() { Customer = request.Customer, Language = request.Language, Store = request.Store });

            //top menu topics
            var topicModel = (await _topicService.GetAllTopics(request.Store.Id))
                .Where(t => t.IncludeInTopMenu)
                .Select(t => new TopMenuModel.TopMenuTopicModel {
                    Id = t.Id,
                    Name = t.GetLocalized(x => x.Title, request.Language.Id),
                    SeName = t.GetSeName(request.Language.Id)
                }).ToList();

            string manufacturerCacheKey = string.Format(ModelCacheEventConst.MANUFACTURER_NAVIGATION_MENU,
                request.Language.Id, request.Store.Id);

            var cachedManufacturerModel = await _cacheManager.GetAsync(manufacturerCacheKey, async () =>
                    (await _manufacturerService.GetAllManufacturers(storeId: request.Store.Id))
                    .Where(x => x.IncludeInTopMenu)
                    .Select(t => new TopMenuModel.TopMenuManufacturerModel {
                        Id = t.Id,
                        Name = t.GetLocalized(x => x.Name, request.Language.Id),
                        Icon = t.Icon,
                        SeName = t.GetSeName(request.Language.Id)
                    })
                    .ToList()
                );

            var model = new TopMenuModel {
                Categories = cachedCategoriesModel,
                Topics = topicModel,
                Manufacturers = cachedManufacturerModel,
                NewProductsEnabled = _catalogSettings.NewProductsEnabled,
                BlogEnabled = _blogSettings.Enabled,
                ForumEnabled = _forumSettings.ForumsEnabled,
                DisplayHomePageMenu = _menuItemSettings.DisplayHomePageMenu,
                DisplayNewProductsMenu = _menuItemSettings.DisplayNewProductsMenu,
                DisplaySearchMenu = _menuItemSettings.DisplaySearchMenu,
                DisplayCustomerMenu = _menuItemSettings.DisplayCustomerMenu,
                DisplayBlogMenu = _menuItemSettings.DisplayBlogMenu,
                DisplayForumsMenu = _menuItemSettings.DisplayForumsMenu,
                DisplayContactUsMenu = _menuItemSettings.DisplayContactUsMenu
            };

            return model;

        }
    }
}