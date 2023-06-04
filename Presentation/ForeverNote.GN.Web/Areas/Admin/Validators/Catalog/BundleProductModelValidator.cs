﻿using FluentValidation;
using ForeverNote.Core;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Extensions;
using ForeverNote.Web.Areas.Admin.Models.Catalog;
using System.Collections.Generic;
using System.Linq;

namespace ForeverNote.Web.Areas.Admin.Validators.Catalog
{
    public class BundleProductModelValidator : BaseForeverNoteValidator<ProductModel.BundleProductModel>
    {
        public BundleProductModelValidator(
            IEnumerable<IValidatorConsumer<ProductModel.BundleProductModel>> validators,
            ILocalizationService localizationService, IProductService productService, IWorkContext workContext)
            : base(validators)
        {
            if (workContext.CurrentCustomer.IsStaff())
            {
                RuleFor(x => x).MustAsync(async (x, y, context) =>
                {
                    var product = await productService.GetProductById(x.ProductBundleId);
                    if (product != null)
                        if (!product.AccessToEntityByStore(workContext.CurrentCustomer.StaffStoreId))
                            return false;

                    return true;
                }).WithMessage(localizationService.GetResource("Admin.Catalog.Products.Permisions"));
            }
            else if (workContext.CurrentVendor != null)
            {
                RuleFor(x => x).MustAsync(async (x, y, context) =>
                {
                    var product = await productService.GetProductById(x.ProductId);
                    if (product != null)
                        if (product != null && product.VendorId != workContext.CurrentVendor.Id)
                            return false;

                    return true;
                }).WithMessage(localizationService.GetResource("Admin.Catalog.Products.Permisions"));
            }
        }
    }
}