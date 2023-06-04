using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mapping;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Models.Discounts;
using ForeverNote.Web.Areas.Admin.Validators.Catalog;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Catalog
{
    [Validator(typeof(ProductValidator))]
    public partial class ProductModel : BaseForeverNoteEntityModel, ILocalizedModel<ProductLocalizedModel>, IAclMappingModel, IStoreMappingModel
    {
        public ProductModel()
        {
            Locales = new List<ProductLocalizedModel>();
            ProductPictureModels = new List<ProductPictureModel>();
            CopyProductModel = new CopyProductModel();
            AvailableBasepriceUnits = new List<SelectListItem>();
            AvailableBasepriceBaseUnits = new List<SelectListItem>();
            AvailableProductTemplates = new List<SelectListItem>();
            AvailableVendors = new List<SelectListItem>();
            AvailableTaxCategories = new List<SelectListItem>();
            AvailableDeliveryDates = new List<SelectListItem>();
            AvailableWarehouses = new List<SelectListItem>();
            AvailableCategories = new List<SelectListItem>();
            AvailableManufacturers = new List<SelectListItem>();
            AvailableProductAttributes = new List<SelectListItem>();
            AvailableUnits = new List<SelectListItem>();
            AddPictureModel = new ProductPictureModel();
            AvailableStores = new List<StoreModel>();
            AvailableCustomerRoles = new List<CustomerRoleModel>();
            AddSpecificationAttributeModel = new AddProductSpecificationAttributeModel();
            ProductWarehouseInventoryModels = new List<ProductWarehouseInventoryModel>();
            CalendarModel = new GenerateCalendarModel();
        }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.ID")]
        public override string Id { get; set; }

        //picture thumbnail
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.PictureThumbnailUrl")]
        public string PictureThumbnailUrl { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.ProductType")]
        public int ProductTypeId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.ProductType")]
        public string ProductTypeName { get; set; }
        public bool AuctionEnded { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.AssociatedToProductName")]
        public string AssociatedToProductId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.AssociatedToProductName")]
        public string AssociatedToProductName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.VisibleIndividually")]
        public bool VisibleIndividually { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.ProductTemplate")]
        public string ProductTemplateId { get; set; }
        public IList<SelectListItem> AvailableProductTemplates { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Name")]
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.ShortDescription")]
        public string ShortDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.FullDescription")]
        public string FullDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Flag")]
        public string Flag { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.AdminComment")]
        public string AdminComment { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Vendor")]
        public string VendorId { get; set; }
        public IList<SelectListItem> AvailableVendors { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.ShowOnHomePage")]
        public bool ShowOnHomePage { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.MetaKeywords")]
        
        public string MetaKeywords { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.MetaDescription")]
        
        public string MetaDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.MetaTitle")]
        
        public string MetaTitle { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.SeName")]
        
        public string SeName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.AllowCustomerReviews")]
        public bool AllowCustomerReviews { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.ProductTags")]
        public string ProductTags { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Sku")]
        
        public string Sku { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.ManufacturerPartNumber")]
        
        public string ManufacturerPartNumber { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.GTIN")]
        
        public virtual string Gtin { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.IsGiftCard")]
        public bool IsGiftCard { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.GiftCardType")]
        public int GiftCardTypeId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.OverriddenGiftCardAmount")]
        [UIHint("DecimalNullable")]
        public decimal? OverriddenGiftCardAmount { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.RequireOtherProducts")]
        public bool RequireOtherProducts { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.RequiredProductIds")]
        public string RequiredProductIds { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.AutomaticallyAddRequiredProducts")]
        public bool AutomaticallyAddRequiredProducts { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.IsDownload")]
        public bool IsDownload { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Download")]
        [UIHint("Download")]
        public string DownloadId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.UnlimitedDownloads")]
        public bool UnlimitedDownloads { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.MaxNumberOfDownloads")]
        public int MaxNumberOfDownloads { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.DownloadExpirationDays")]
        [UIHint("Int32Nullable")]
        public int? DownloadExpirationDays { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.DownloadActivationType")]
        public int DownloadActivationTypeId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.HasSampleDownload")]
        public bool HasSampleDownload { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.SampleDownload")]
        [UIHint("Download")]
        public string SampleDownloadId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.HasUserAgreement")]
        public bool HasUserAgreement { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.UserAgreementText")]
        
        public string UserAgreementText { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.IsRecurring")]
        public bool IsRecurring { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.RecurringCycleLength")]
        public int RecurringCycleLength { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.RecurringCyclePeriod")]
        public int RecurringCyclePeriodId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.RecurringTotalCycles")]
        public int RecurringTotalCycles { get; set; }

        //calendar
        public GenerateCalendarModel CalendarModel { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.IsShipEnabled")]
        public bool IsShipEnabled { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.IsFreeShipping")]
        public bool IsFreeShipping { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.ShipSeparately")]
        public bool ShipSeparately { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.AdditionalShippingCharge")]
        public decimal AdditionalShippingCharge { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.DeliveryDate")]
        public string DeliveryDateId { get; set; }
        public IList<SelectListItem> AvailableDeliveryDates { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.IsTaxExempt")]
        public bool IsTaxExempt { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.TaxCategory")]
        public string TaxCategoryId { get; set; }
        public IList<SelectListItem> AvailableTaxCategories { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.IsTelecommunicationsOrBroadcastingOrElectronicServices")]
        public bool IsTele { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.ManageInventoryMethod")]
        public int ManageInventoryMethodId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.UseMultipleWarehouses")]
        public bool UseMultipleWarehouses { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Warehouse")]
        public string WarehouseId { get; set; }
        public IList<SelectListItem> AvailableWarehouses { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.StockQuantity")]
        public int StockQuantity { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.StockQuantity")]
        public string StockQuantityStr { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.DisplayStockAvailability")]
        public bool DisplayStockAvailability { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.DisplayStockQuantity")]
        public bool DisplayStockQuantity { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.MinStockQuantity")]
        public int MinStockQuantity { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.LowStockActivity")]
        public int LowStockActivityId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.NotifyAdminForQuantityBelow")]
        public int NotifyAdminForQuantityBelow { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.BackorderMode")]
        public int BackorderModeId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.AllowBackInStockSubscriptions")]
        public bool AllowBackInStockSubscriptions { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.OrderMinimumQuantity")]
        public int OrderMinimumQuantity { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.OrderMaximumQuantity")]
        public int OrderMaximumQuantity { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.AllowedQuantities")]
        public string AllowedQuantities { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.AllowAddingOnlyExistingAttributeCombinations")]
        public bool AllowAddingOnlyExistingAttributeCombinations { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.NotReturnable")]
        public bool NotReturnable { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.DisableBuyButton")]
        public bool DisableBuyButton { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.DisableWishlistButton")]
        public bool DisableWishlistButton { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.AvailableForPreOrder")]
        public bool AvailableForPreOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.PreOrderAvailabilityStartDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? PreOrderAvailabilityStartDateTime { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.CallForPrice")]
        public bool CallForPrice { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Price")]
        public decimal Price { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.OldPrice")]
        public decimal OldPrice { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.CatalogPrice")]
        public decimal CatalogPrice { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.StartPrice")]
        public decimal StartPrice { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.ProductCost")]
        public decimal ProductCost { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.CustomerEntersPrice")]
        public bool CustomerEntersPrice { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.MinimumCustomerEnteredPrice")]
        public decimal MinimumCustomerEnteredPrice { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.MaximumCustomerEnteredPrice")]
        public decimal MaximumCustomerEnteredPrice { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.BasepriceEnabled")]
        public bool BasepriceEnabled { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.BasepriceAmount")]
        public decimal BasepriceAmount { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.BasepriceUnit")]
        public string BasepriceUnitId { get; set; }
        public IList<SelectListItem> AvailableBasepriceUnits { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.BasepriceBaseAmount")]
        public decimal BasepriceBaseAmount { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.BasepriceBaseUnit")]
        public string BasepriceBaseUnitId { get; set; }
        public IList<SelectListItem> AvailableBasepriceBaseUnits { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.MarkAsNew")]
        public bool MarkAsNew { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.MarkAsNewStartDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? MarkAsNewStartDateTime { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.MarkAsNewEndDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? MarkAsNewEndDateTime { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Unit")]
        public string UnitId { get; set; }
        public IList<SelectListItem> AvailableUnits { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Weight")]
        public decimal Weight { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Length")]
        public decimal Length { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Width")]
        public decimal Width { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Height")]
        public decimal Height { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.AvailableStartDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? AvailableStartDateTime { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.AvailableEndDateTime")]
        [UIHint("DateTimeNullable")]
        public DateTime? AvailableEndDateTime { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.DisplayOrderCategory")]
        public int DisplayOrderCategory { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.DisplayOrderManufacturer")]
        public int DisplayOrderManufacturer { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.DisplayOrderOnSale")]
        public int OnSale { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Published")]
        public bool Published { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.CreatedOn")]
        public DateTime? CreatedOn { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.UpdatedOn")]
        public DateTime? UpdatedOn { get; set; }
        public long Ticks { get; set; }

        public string PrimaryStoreCurrencyCode { get; set; }
        public string BaseDimensionIn { get; set; }
        public string BaseWeightIn { get; set; }

        public IList<ProductLocalizedModel> Locales { get; set; }


        //ACL (customer roles)
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.SubjectToAcl")]
        public bool SubjectToAcl { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.AclCustomerRoles")]
        public List<CustomerRoleModel> AvailableCustomerRoles { get; set; }
        public string[] SelectedCustomerRoleIds { get; set; }

        //Store mapping
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }


        //vendor
        public bool IsLoggedInAsVendor { get; set; }



        //categories
        public IList<SelectListItem> AvailableCategories { get; set; }
        //manufacturers
        public IList<SelectListItem> AvailableManufacturers { get; set; }
        //product attributes
        public IList<SelectListItem> AvailableProductAttributes { get; set; }
        


        //pictures
        public ProductPictureModel AddPictureModel { get; set; }
        public IList<ProductPictureModel> ProductPictureModels { get; set; }

        //discounts
        public List<DiscountModel> AvailableDiscounts { get; set; }
        public string[] SelectedDiscountIds { get; set; }
        //add specification attribute model
        public AddProductSpecificationAttributeModel AddSpecificationAttributeModel { get; set; }
        //multiple warehouses
        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductWarehouseInventory")]
        public IList<ProductWarehouseInventoryModel> ProductWarehouseInventoryModels { get; set; }

        //copy product
        public CopyProductModel CopyProductModel { get; set; }
        
        #region Nested classes

        public partial class AddRequiredProductModel : BaseForeverNoteModel
        {
            public AddRequiredProductModel()
            {
                AvailableCategories = new List<SelectListItem>();
                AvailableManufacturers = new List<SelectListItem>();
                AvailableStores = new List<SelectListItem>();
                AvailableVendors = new List<SelectListItem>();
                AvailableProductTypes = new List<SelectListItem>();
            }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]
            
            public string SearchProductName { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
            public string SearchCategoryId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
            public string SearchManufacturerId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
            public string SearchStoreId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchVendor")]
            public string SearchVendorId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchProductType")]
            public int SearchProductTypeId { get; set; }

            public IList<SelectListItem> AvailableCategories { get; set; }
            public IList<SelectListItem> AvailableManufacturers { get; set; }
            public IList<SelectListItem> AvailableStores { get; set; }
            public IList<SelectListItem> AvailableVendors { get; set; }
            public IList<SelectListItem> AvailableProductTypes { get; set; }

            //vendor
            public bool IsLoggedInAsVendor { get; set; }
        }

        [Validator(typeof(AddProductSpecificationAttributeModelValidator))]
        public partial class AddProductSpecificationAttributeModel : BaseForeverNoteModel
        {
            public AddProductSpecificationAttributeModel()
            {
                AvailableAttributes = new List<SelectListItem>();
                AvailableOptions = new List<SelectListItem>();
            }
            
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.SpecificationAttribute")]
            public string SpecificationAttributeId { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.AttributeType")]
            public int AttributeTypeId { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.SpecificationAttributeOption")]
            public string SpecificationAttributeOptionId { get; set; }
            
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.CustomValue")]
            public string CustomValue { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.AllowFiltering")]
            public bool AllowFiltering { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.ShowOnProductPage")]
            public bool ShowOnProductPage { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }

            public string ProductId { get; set; }
            public IList<SelectListItem> AvailableAttributes { get; set; }
            public IList<SelectListItem> AvailableOptions { get; set; }
        }

        [Validator(typeof(ProductPictureModelValidator))]
        public partial class ProductPictureModel : BaseForeverNoteEntityModel
        {
            public string ProductId { get; set; }

            [UIHint("Picture")]
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.Picture")]
            public string PictureId { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.Picture")]
            public string PictureUrl { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.OverrideAltAttribute")]
            
            public string OverrideAltAttribute { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.OverrideTitleAttribute")]
            
            public string OverrideTitleAttribute { get; set; }
        }

        [Validator(typeof(ProductCategoryModelValidator))]
        public partial class ProductCategoryModel : BaseForeverNoteEntityModel
        {
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Categories.Fields.Category")]
            public string Category { get; set; }

            public string ProductId { get; set; }

            public string CategoryId { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Categories.Fields.IsFeaturedProduct")]
            public bool IsFeaturedProduct { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Categories.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }
        }

        [Validator(typeof(ProductManufacturerModelValidator))]
        public partial class ProductManufacturerModel : BaseForeverNoteEntityModel
        {
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Manufacturers.Fields.Manufacturer")]
            public string Manufacturer { get; set; }

            public string ProductId { get; set; }

            public string ManufacturerId { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Manufacturers.Fields.IsFeaturedProduct")]
            public bool IsFeaturedProduct { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Manufacturers.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }
        }

        [Validator(typeof(RelatedProductModelValidator))]
        public partial class RelatedProductModel : BaseForeverNoteEntityModel
        {
            public string ProductId1 { get; set; }
            public string ProductId2 { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.RelatedProducts.Fields.Product")]
            public string Product2Name { get; set; }
            
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.RelatedProducts.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }
        }
        [Validator(typeof(AddRelatedProductModelValidator))]
        public partial class AddRelatedProductModel : BaseForeverNoteModel
        {
            public AddRelatedProductModel()
            {
                AvailableCategories = new List<SelectListItem>();
                AvailableManufacturers = new List<SelectListItem>();
                AvailableStores = new List<SelectListItem>();
                AvailableVendors = new List<SelectListItem>();
                AvailableProductTypes = new List<SelectListItem>();
            }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]
            
            public string SearchProductName { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
            public string SearchCategoryId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
            public string SearchManufacturerId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
            public string SearchStoreId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchVendor")]
            public string SearchVendorId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchProductType")]
            public int SearchProductTypeId { get; set; }

            public IList<SelectListItem> AvailableCategories { get; set; }
            public IList<SelectListItem> AvailableManufacturers { get; set; }
            public IList<SelectListItem> AvailableStores { get; set; }
            public IList<SelectListItem> AvailableVendors { get; set; }
            public IList<SelectListItem> AvailableProductTypes { get; set; }

            public string ProductId { get; set; }

            public string[] SelectedProductIds { get; set; }

            //vendor
            public bool IsLoggedInAsVendor { get; set; }
        }

        [Validator(typeof(SimilarProductModelValidator))]
        public partial class SimilarProductModel : BaseForeverNoteEntityModel
        {
            public string ProductId1 { get; set; }
            public string ProductId2 { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.SimilarProducts.Fields.Product")]
            public string Product2Name { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.SimilarProducts.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }
        }

        [Validator(typeof(AddSimilarProductModelValidator))]
        public partial class AddSimilarProductModel : BaseForeverNoteModel
        {
            public AddSimilarProductModel()
            {
                AvailableCategories = new List<SelectListItem>();
                AvailableManufacturers = new List<SelectListItem>();
                AvailableStores = new List<SelectListItem>();
                AvailableVendors = new List<SelectListItem>();
                AvailableProductTypes = new List<SelectListItem>();
            }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]

            public string SearchProductName { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
            public string SearchCategoryId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
            public string SearchManufacturerId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
            public string SearchStoreId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchVendor")]
            public string SearchVendorId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchProductType")]
            public int SearchProductTypeId { get; set; }

            public IList<SelectListItem> AvailableCategories { get; set; }
            public IList<SelectListItem> AvailableManufacturers { get; set; }
            public IList<SelectListItem> AvailableStores { get; set; }
            public IList<SelectListItem> AvailableVendors { get; set; }
            public IList<SelectListItem> AvailableProductTypes { get; set; }

            public string ProductId { get; set; }

            public string[] SelectedProductIds { get; set; }

            //vendor
            public bool IsLoggedInAsVendor { get; set; }
        }

        [Validator(typeof(BundleProductModelValidator))]
        public partial class BundleProductModel : BaseForeverNoteEntityModel
        {
            public string ProductBundleId { get; set; }
            public string ProductId { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.BundleProducts.Fields.Product")]
            public string ProductName { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.BundleProducts.Fields.Quantity")]
            public int Quantity { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.BundleProducts.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }
        }

        [Validator(typeof(AddBundleProductModelValidator))]
        public partial class AddBundleProductModel : BaseForeverNoteModel
        {
            public AddBundleProductModel()
            {
                AvailableCategories = new List<SelectListItem>();
                AvailableManufacturers = new List<SelectListItem>();
                AvailableStores = new List<SelectListItem>();
                AvailableVendors = new List<SelectListItem>();
            }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]

            public string SearchProductName { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
            public string SearchCategoryId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
            public string SearchManufacturerId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
            public string SearchStoreId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchVendor")]
            public string SearchVendorId { get; set; }
            public IList<SelectListItem> AvailableCategories { get; set; }
            public IList<SelectListItem> AvailableManufacturers { get; set; }
            public IList<SelectListItem> AvailableStores { get; set; }
            public IList<SelectListItem> AvailableVendors { get; set; }

            public string ProductId { get; set; }

            public string[] SelectedProductIds { get; set; }

            //vendor
            public bool IsLoggedInAsVendor { get; set; }
        }
        [Validator(typeof(AssociatedProductModelValidator))]
        public partial class AssociatedProductModel : BaseForeverNoteEntityModel
        {
            public string ProductId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.AssociatedProducts.Fields.Product")]
            public string ProductName { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.AssociatedProducts.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }
        }
        [Validator(typeof(AddAssociatedProductModelValidator))]
        public partial class AddAssociatedProductModel : BaseForeverNoteModel
        {
            public AddAssociatedProductModel()
            {
                AvailableCategories = new List<SelectListItem>();
                AvailableManufacturers = new List<SelectListItem>();
                AvailableStores = new List<SelectListItem>();
                AvailableVendors = new List<SelectListItem>();
                AvailableProductTypes = new List<SelectListItem>();
            }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]
            
            public string SearchProductName { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
            public string SearchCategoryId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
            public string SearchManufacturerId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
            public string SearchStoreId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchVendor")]
            public string SearchVendorId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchProductType")]
            public int SearchProductTypeId { get; set; }

            public IList<SelectListItem> AvailableCategories { get; set; }
            public IList<SelectListItem> AvailableManufacturers { get; set; }
            public IList<SelectListItem> AvailableStores { get; set; }
            public IList<SelectListItem> AvailableVendors { get; set; }
            public IList<SelectListItem> AvailableProductTypes { get; set; }

            public string ProductId { get; set; }

            public string[] SelectedProductIds { get; set; }

            //vendor
            public bool IsLoggedInAsVendor { get; set; }
        }

        [Validator(typeof(CrossSellProductModelValidator))]
        public partial class CrossSellProductModel : BaseForeverNoteEntityModel
        {
            public string ProductId { get; set; }
            public string ProductId2 { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.CrossSells.Fields.Product")]
            public string Product2Name { get; set; }
        }
        [Validator(typeof(AddCrossSellProductModelValidator))]
        public partial class AddCrossSellProductModel : BaseForeverNoteModel
        {
            public AddCrossSellProductModel()
            {
                AvailableCategories = new List<SelectListItem>();
                AvailableManufacturers = new List<SelectListItem>();
                AvailableStores = new List<SelectListItem>();
                AvailableVendors = new List<SelectListItem>();
                AvailableProductTypes = new List<SelectListItem>();
            }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]
            
            public string SearchProductName { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
            public string SearchCategoryId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
            public string SearchManufacturerId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
            public string SearchStoreId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchVendor")]
            public string SearchVendorId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchProductType")]
            public int SearchProductTypeId { get; set; }

            public IList<SelectListItem> AvailableCategories { get; set; }
            public IList<SelectListItem> AvailableManufacturers { get; set; }
            public IList<SelectListItem> AvailableStores { get; set; }
            public IList<SelectListItem> AvailableVendors { get; set; }
            public IList<SelectListItem> AvailableProductTypes { get; set; }

            public string ProductId { get; set; }

            public string[] SelectedProductIds { get; set; }

            //vendor
            public bool IsLoggedInAsVendor { get; set; }
        }

        [Validator(typeof(TierPriceModelValidator))]
        public partial class TierPriceModel : BaseForeverNoteEntityModel
        {

            public TierPriceModel()
            {
                AvailableStores = new List<SelectListItem>();
                AvailableCustomerRoles = new List<SelectListItem>();
            }
            public string ProductId { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.TierPrices.Fields.CustomerRole")]
            public string CustomerRoleId { get; set; }
            public IList<SelectListItem> AvailableCustomerRoles { get; set; }
            public string CustomerRole { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.TierPrices.Fields.Store")]
            public string StoreId { get; set; }
            public IList<SelectListItem> AvailableStores { get; set; }
            public string Store { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.TierPrices.Fields.Quantity")]
            public int Quantity { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.TierPrices.Fields.Price")]
            public decimal Price { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.TierPrices.Fields.StartDateTime")]
            [UIHint("DateTimeNullable")]
            public DateTime? StartDateTime { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.TierPrices.Fields.EndDateTime")]
            [UIHint("DateTimeNullable")]
            public DateTime? EndDateTime { get; set; }

        }

        public partial class ProductWarehouseInventoryModel : BaseForeverNoteModel
        {
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductWarehouseInventory.Fields.Warehouse")]
            public string WarehouseId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductWarehouseInventory.Fields.Warehouse")]
            public string WarehouseName { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductWarehouseInventory.Fields.WarehouseUsed")]
            public bool WarehouseUsed { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductWarehouseInventory.Fields.StockQuantity")]
            public int StockQuantity { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductWarehouseInventory.Fields.ReservedQuantity")]
            public int ReservedQuantity { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductWarehouseInventory.Fields.PlannedQuantity")]
            public int PlannedQuantity { get; set; }
        }
        public partial class ReservationModel : BaseForeverNoteEntityModel
        {
            public string ReservationId { get; set; }
            public DateTime Date { get; set; }
            public string Resource { get; set; }
            public string Parameter { get; set; }
            public string OrderId { get; set; }
            public string ProductId { get; set; }
            public string Duration { get; set; }
        }

        public partial class BidModel : BaseForeverNoteEntityModel
        {
            public string ProductId { get; set; }
            public string BidId { get; set; }
            public DateTime Date { get; set; }
            public string CustomerId { get; set; }
            public string Email { get; set; }
            public string Amount { get; set; }
            public string OrderId { get; set; }
        }

        public partial class GenerateCalendarModel : BaseForeverNoteModel
        {

            public GenerateCalendarModel()
            {
                Interval = 1;
                Quantity = 1;
            }
            public string ProductId { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.StartDate")]
            [UIHint("DateNullable")]
            public DateTime? StartDate { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.StartTime")]
            [UIHint("Time")]
            public DateTime StartTime { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.EndDate")]
            [UIHint("DateNullable")]
            public DateTime? EndDate { get; set; }
            [UIHint("Time")]

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.EndTime")]
            public DateTime EndTime { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.Interval")]
            public int Interval { get; set; } = 1;
            public int IntervalUnit { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.IncBothDate")]
            public bool IncBothDate { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.Quantity")]
            public int Quantity { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.Resource")]
            public string Resource { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.Parameter")]
            public string Parameter { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.Monday")]
            public bool Monday { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.Tuesday")]
            public bool Tuesday { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.Wednesday")]
            public bool Wednesday { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.Thursday")]
            public bool Thursday { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.Friday")]
            public bool Friday { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.Saturday")]
            public bool Saturday { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Calendar.Sunday")]
            public bool Sunday { get; set; }

        }

        [Validator(typeof(ProductAttributeMappingModelValidator))]
        public partial class ProductAttributeMappingModel : BaseForeverNoteEntityModel
        {
            public ProductAttributeMappingModel()
            {
                AvailableProductAttribute = new List<SelectListItem>();
            }
            public string ProductId { get; set; }

            public string ProductAttributeId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Fields.Attribute")]
            public string ProductAttribute { get; set; }
            public IList<SelectListItem> AvailableProductAttribute { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Fields.TextPrompt")]
            public string TextPrompt { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Fields.IsRequired")]
            public bool IsRequired { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Fields.ShowOnCatalogPage")]
            public bool ShowOnCatalogPage { get; set; }
            public int AttributeControlTypeId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Fields.AttributeControlType")]
            public string AttributeControlType { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }

            public bool ShouldHaveValues { get; set; }
            public int TotalValues { get; set; }

            //validation fields
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.ValidationRules")]
            public bool ValidationRulesAllowed { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.ValidationRules.MinLength")]
            [UIHint("Int32Nullable")]
            public int? ValidationMinLength { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.ValidationRules.MaxLength")]
            [UIHint("Int32Nullable")]
            public int? ValidationMaxLength { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.ValidationRules.FileAllowedExtensions")]
            
            public string ValidationFileAllowedExtensions { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.ValidationRules.FileMaximumSize")]
            [UIHint("Int32Nullable")]
            public int? ValidationFileMaximumSize { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.ValidationRules.DefaultValue")]
            
            public string DefaultValue { get; set; }
            public string ValidationRulesString { get; set; }

            //condition
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Condition")]
            public bool ConditionAllowed { get; set; }
            public string ConditionString { get; set; }
        }
        public partial class ProductAttributeValueListModel : BaseForeverNoteModel
        {
            public string ProductId { get; set; }

            public string ProductName { get; set; }

            public string ProductAttributeMappingId { get; set; }

            public string ProductAttributeName { get; set; }
        }
        [Validator(typeof(ProductAttributeValueModelValidator))]
        public partial class ProductAttributeValueModel : BaseForeverNoteEntityModel, ILocalizedModel<ProductAttributeValueLocalizedModel>
        {
            public ProductAttributeValueModel()
            {
                ProductPictureModels = new List<ProductPictureModel>();
                Locales = new List<ProductAttributeValueLocalizedModel>();
            }

            public string ProductAttributeMappingId { get; set; }
            public string ProductId { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.AttributeValueType")]
            public int AttributeValueTypeId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.AttributeValueType")]
            public string AttributeValueTypeName { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.AssociatedProduct")]
            public string AssociatedProductId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.AssociatedProduct")]
            public string AssociatedProductName { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.Name")]
            
            public string Name { get; set; }
            
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.ColorSquaresRgb")]
            
            public string ColorSquaresRgb { get; set; }
            public bool DisplayColorSquaresRgb { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.ImageSquaresPicture")]
            [UIHint("Picture")]
            public string ImageSquaresPictureId { get; set; }
            public bool DisplayImageSquaresPicture { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.PriceAdjustment")]
            public decimal PriceAdjustment { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.PriceAdjustment")]
            //used only on the values list page
            public string PriceAdjustmentStr { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.WeightAdjustment")]
            public decimal WeightAdjustment { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.WeightAdjustment")]
            //used only on the values list page
            public string WeightAdjustmentStr { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.Cost")]
            public decimal Cost { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.Quantity")]
            public int Quantity { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.IsPreSelected")]
            public bool IsPreSelected { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.DisplayOrder")]
            public int DisplayOrder { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.Picture")]
            public string PictureId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.Picture")]
            public string PictureThumbnailUrl { get; set; }

            public IList<ProductPictureModel> ProductPictureModels { get; set; }
            public IList<ProductAttributeValueLocalizedModel> Locales { get; set; }

            #region Nested classes

            public partial class AssociateProductToAttributeValueModel : BaseForeverNoteModel
            {
                public AssociateProductToAttributeValueModel()
                {
                    AvailableCategories = new List<SelectListItem>();
                    AvailableManufacturers = new List<SelectListItem>();
                    AvailableStores = new List<SelectListItem>();
                    AvailableVendors = new List<SelectListItem>();
                    AvailableProductTypes = new List<SelectListItem>();
                }

                [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]
                
                public string SearchProductName { get; set; }
                [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
                public string SearchCategoryId { get; set; }
                [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
                public string SearchManufacturerId { get; set; }
                [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
                public string SearchStoreId { get; set; }
                [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchVendor")]
                public string SearchVendorId { get; set; }
                [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchProductType")]
                public int SearchProductTypeId { get; set; }

                public IList<SelectListItem> AvailableCategories { get; set; }
                public IList<SelectListItem> AvailableManufacturers { get; set; }
                public IList<SelectListItem> AvailableStores { get; set; }
                public IList<SelectListItem> AvailableVendors { get; set; }
                public IList<SelectListItem> AvailableProductTypes { get; set; }
                
                //vendor
                public bool IsLoggedInAsVendor { get; set; }


                public string AssociatedToProductId { get; set; }
            }


            #endregion
        }
        public partial class ActivityLogModel : BaseForeverNoteEntityModel
        {
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ActivityLog.ActivityLogType")]
            public string ActivityLogTypeName { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ActivityLog.Comment")]
            public string Comment { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ActivityLog.CreatedOn")]
            public DateTime CreatedOn { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ActivityLog.Customer")]
            public string CustomerId { get; set; }
            public string CustomerEmail { get; set; }
        }
        public partial class ProductAttributeValueLocalizedModel : ILocalizedModelLocal
        {
            public string LanguageId { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.Name")]
            
            public string Name { get; set; }
        }
        public partial class ProductAttributeCombinationModel : BaseForeverNoteEntityModel
        {
            public string ProductId { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.AttributeCombinations.Fields.Attributes")]
            
            public string AttributesXml { get; set; }

            
            public string Warnings { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.AttributeCombinations.Fields.StockQuantity")]
            public int StockQuantity { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.AttributeCombinations.Fields.AllowOutOfStockOrders")]
            public bool AllowOutOfStockOrders { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.AttributeCombinations.Fields.Sku")]
            public string Sku { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.AttributeCombinations.Fields.ManufacturerPartNumber")]
            public string ManufacturerPartNumber { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.AttributeCombinations.Fields.Gtin")]
            public string Gtin { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.AttributeCombinations.Fields.OverriddenPrice")]
            [UIHint("DecimalNullable")]
            public decimal? OverriddenPrice { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.ProductAttributes.AttributeCombinations.Fields.NotifyAdminForQuantityBelow")]
            public int NotifyAdminForQuantityBelow { get; set; }

        }
        public partial class ProductAttributeCombinationTierPricesModel : BaseForeverNoteEntityModel
        {
            public string StoreId { get; set; }
            public string Store { get; set; }

            /// <summary>
            /// Gets or sets the customer role identifier
            /// </summary>
            public string CustomerRoleId { get; set; }
            public string CustomerRole { get; set; }

            /// <summary>
            /// Gets or sets the quantity
            /// </summary>
            public int Quantity { get; set; }

            /// <summary>
            /// Gets or sets the price
            /// </summary>
            public decimal Price { get; set; }
        }

        #endregion
    }

    public partial class ProductLocalizedModel : ILocalizedModelLocal, ISlugModelLocal
    {
        public string LanguageId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.ShortDescription")]
        
        public string ShortDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.FullDescription")]
        
        public string FullDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.MetaKeywords")]
        
        public string MetaKeywords { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.MetaDescription")]
        
        public string MetaDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.MetaTitle")]
        
        public string MetaTitle { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Products.Fields.SeName")]
        
        public string SeName { get; set; }
    }
}