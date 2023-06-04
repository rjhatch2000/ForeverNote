﻿using FluentValidation;
using ForeverNote.Core;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Web.Framework.Validators;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Areas.Admin.Extensions;
using ForeverNote.Web.Areas.Admin.Models.Catalog;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Validators.Catalog
{
    public class ProductAttributeValueModelValidator : BaseForeverNoteValidator<ProductModel.ProductAttributeValueModel>
    {
        public ProductAttributeValueModelValidator(
            IEnumerable<IValidatorConsumer<ProductModel.ProductAttributeValueModel>> validators,
            ILocalizationService localizationService, IProductService productService, IWorkContext workContext)
            : base(validators)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.Name.Required"));

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(1)
                .WithMessage(localizationService.GetResource("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.Quantity.GreaterThanOrEqualTo1"))
                .When(x => x.AttributeValueTypeId == (int)AttributeValueType.AssociatedToProduct);

            if (workContext.CurrentCustomer.IsStaff())
            {
                RuleFor(x => x).MustAsync(async (x, y, context) =>
                {
                    var product = await productService.GetProductById(x.ProductId);
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