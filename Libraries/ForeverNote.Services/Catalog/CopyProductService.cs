using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Media;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Media;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Catalog
{
    /// <summary>
    /// Copy Product service
    /// </summary>
    public partial class CopyProductService : ICopyProductService
    {
        #region Fields

        private readonly IProductService _productService;
        private readonly ILanguageService _languageService;
        private readonly IPictureService _pictureService;
        private readonly IDownloadService _downloadService;
        #endregion

        #region Ctor

        public CopyProductService(IProductService productService,
            ILanguageService languageService,
            IPictureService pictureService,
            IDownloadService downloadService
        )
        {
            _productService = productService;
            _languageService = languageService;
            _pictureService = pictureService;
            _downloadService = downloadService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a copy of product with all depended data
        /// </summary>
        /// <param name="product">The product to copy</param>
        /// <param name="newName">The name of product duplicate</param>
        /// <param name="isPublished">A value indicating whether the product duplicate should be published</param>
        /// <param name="copyImages">A value indicating whether the product images should be copied</param>
        /// <param name="copyAssociatedProducts">A value indicating whether the copy associated products</param>
        /// <returns>Product copy</returns>
        public virtual async Task<Product> CopyProduct(Product product, string newName,
            bool copyImages = true, bool copyAssociatedProducts = true)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            if (String.IsNullOrEmpty(newName))
                throw new ArgumentException("Product name is required");

            // product
            var productCopy = new Product
            {
                Name = newName,
                ShortDescription = product.ShortDescription,
                FullDescription = product.FullDescription,
                Flag = product.Flag,
                AdminComment = product.AdminComment,
                ShowOnHomePage = product.ShowOnHomePage,
                MetaKeywords = product.MetaKeywords,
                MetaDescription = product.MetaDescription,
                MetaTitle = product.MetaTitle,
                IsRecurring = product.IsRecurring,
                RecurringCycleLength = product.RecurringCycleLength,
                RecurringCyclePeriod = product.RecurringCyclePeriod,
                RecurringTotalCycles = product.RecurringTotalCycles,
                DeliveryDateId = product.DeliveryDateId,
                AllowBackInStockSubscriptions = product.AllowBackInStockSubscriptions,
                MarkAsNew = product.MarkAsNew,
                MarkAsNewStartDateTimeUtc = product.MarkAsNewStartDateTimeUtc,
                MarkAsNewEndDateTimeUtc = product.MarkAsNewEndDateTimeUtc,
                DisplayOrder = product.DisplayOrder,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                Locales = product.Locales,
                CustomerRoles = product.CustomerRoles,                
            };

            // product <-> categories mappings
            foreach (var productCategory in product.ProductCategories)
            {
                productCopy.ProductCategories.Add(productCategory);
            }

            //product tags
            foreach (var productTag in product.ProductTags)
            {
                productCopy.ProductTags.Add(productTag);
            }

            //validate search engine name
            await _productService.InsertProduct(productCopy);

            var languages = await _languageService.GetAllLanguages(true);

            //product pictures
            //variable to store original and new picture identifiers
            int id = 1;
            var originalNewPictureIdentifiers = new Dictionary<string, string>();
            if (copyImages)
            {
                foreach (var productPicture in product.ProductPictures)
                {
                    var picture = await _pictureService.GetPictureById(productPicture.PictureId);
                    var pictureCopy = await _pictureService.InsertPicture(
                        await _pictureService.LoadPictureBinary(picture),
                        picture.MimeType,
                        newName,
                        picture.AltAttribute,
                        picture.TitleAttribute);

                    await _productService.InsertProductPicture(new ProductPicture
                    {
                        ProductId = productCopy.Id,
                        PictureId = pictureCopy.Id,
                        DisplayOrder = productPicture.DisplayOrder
                    });
                    id++;
                    originalNewPictureIdentifiers.Add(picture.Id, pictureCopy.Id);
                }
            }


            return productCopy;
        }

        #endregion
    }
}