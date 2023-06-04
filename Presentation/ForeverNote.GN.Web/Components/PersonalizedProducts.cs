﻿using ForeverNote.Core;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Web.Framework.Components;
using ForeverNote.Services.Catalog;
using ForeverNote.Web.Features.Models.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Web.Components
{
    public class PersonalizedProductsViewComponent : BaseViewComponent
    {
        #region Fields

        private readonly IProductService _productService;
        private readonly IWorkContext _workContext;
        private readonly IMediator _mediator;

        private readonly CatalogSettings _catalogSettings;

        #endregion

        #region Constructors

        public PersonalizedProductsViewComponent(
            IProductService productService,
            IWorkContext workContext,
            IMediator mediator,
            CatalogSettings catalogSettings)
        {
            _productService = productService;
            _workContext = workContext;
            _mediator = mediator;
            _catalogSettings = catalogSettings;
        }

        #endregion

        #region Invoker

        public async Task<IViewComponentResult> InvokeAsync(int? productThumbPictureSize)
        {
            if (!_catalogSettings.PersonalizedProductsEnabled || _catalogSettings.PersonalizedProductsNumber == 0)
                return Content("");

            var products = await _productService.GetPersonalizedProducts(_workContext.CurrentCustomer.Id);
            products = products.Take(_catalogSettings.PersonalizedProductsNumber).ToList();

            if (!products.Any())
                return Content("");

            var model = await _mediator.Send(new GetProductOverview() {
                PreparePictureModel = true,
                PreparePriceModel = true,
                ProductThumbPictureSize = productThumbPictureSize,
                Products = products,
            });

            return View(model);
        }

        #endregion
    }
}
