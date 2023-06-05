using ForeverNote.Core;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Catalog
{
    /// <summary>
    /// Product service
    /// </summary>
    public partial interface IProductService
    {
        #region Products

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="product">Product</param>
        Task DeleteProduct(Product product);

        /// <summary>
        /// Gets all products displayed on the home page
        /// </summary>
        /// <returns>Products</returns>
        Task<IList<Product>> GetAllProductsDisplayedOnHomePage();

        /// <summary>
        /// Gets recommended products for customer roles
        /// </summary>
        /// <returns>Products</returns>
        Task<IList<Product>> GetRecommendedProducts(string[] customerRoleIds);

        /// <summary>
        /// Gets recommended products for customer roles
        /// </summary>
        /// <returns>Products</returns>
        Task<IList<Product>> GetSuggestedProducts(string[] customerTagIds);

        /// <summary>
        /// Gets personalized products for customer 
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        /// <returns>Products</returns>
        Task<IList<Product>> GetPersonalizedProducts(string customerId);

        /// <summary>
        /// Gets product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="fromDB">get data from db (not from cache)</param>
        /// <returns>Product</returns>
        Task<Product> GetProductById(string productId, bool fromDB = false);

        /// <summary>
        /// Gets product from product or product deleted
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product</returns>
        Task<Product> GetProductByIdIncludeArch(string productId);

        /// <summary>
        /// Gets products by identifier
        /// </summary>
        /// <param name="productIds">Product identifiers</param>
        /// <param name="showHidden">Show hidden</param>
        /// <returns>Products</returns>
        Task<IList<Product>> GetProductsByIds(string[] productIds, bool showHidden = false);

        /// <summary>
        /// Inserts a product
        /// </summary>
        /// <param name="product">Product</param>
        Task InsertProduct(Product product);

        /// <summary>
        /// Updates the product
        /// </summary>
        /// <param name="product">Product</param>
        Task UpdateProduct(Product product);

        /// <summary>
        /// Updates most view on the product
        /// </summary>
        /// <param name="productId">ProductId</param>
        /// <param name="qty">Count</param>
        Task UpdateMostView(string productId, int qty);

        /// <summary>
        /// Updates best sellers on the product
        /// </summary>
        /// <param name="productId">ProductId</param>
        /// <param name="qty">Count</param>
        Task UpdateSold(string productId, int qty);

        /// <summary>
        /// Get (visible) product number in certain category
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="categoryIds">Category identifiers</param>
        /// <returns>Product number</returns>
        int GetCategoryProductNumber(Customer customer, IList<string> categoryIds = null);

        /// <summary>
        /// Search products
        /// </summary>
        /// <param name="filterableSpecificationAttributeOptionIds">The specification attribute option identifiers applied to loaded products (all pages)</param>
        /// <param name="loadFilterableSpecificationAttributeOptionIds">A value indicating whether we should load the specification attribute option identifiers applied to loaded products (all pages)</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="categoryIds">Category identifiers</param>
        /// <param name="manufacturerId">Manufacturer identifier; "" to load all records</param>
        /// <param name="vendorId">Vendor identifier; "" to load all records</param>
        /// <param name="warehouseId">Warehouse identifier; "" to load all records</param>
        /// <param name="visibleIndividuallyOnly">A values indicating whether to load only products marked as "visible individually"; "false" to load all records; "true" to load "visible individually" only</param>
        /// <param name="featuredProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers). 0 to load featured products only, 1 to load not featured products only, null to load all products</param>
        /// <param name="priceMin">Minimum price; null to load all records</param>
        /// <param name="priceMax">Maximum price; null to load all records</param>
        /// <param name="productTag">Product tag name; "" to load all records</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search by a specified "keyword" in product descriptions</param>
        /// <param name="searchSku">A value indicating whether to search by a specified "keyword" in product SKU</param>
        /// <param name="searchProductTags">A value indicating whether to search by a specified "keyword" in product tags</param>
        /// <param name="languageId">Language identifier (search for text searching)</param>
        /// <param name="filteredSpecs">Filtered product specification identifiers</param>
        /// <param name="orderBy">Order by</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="overridePublished">
        /// null - process "Published" property according to "showHidden" parameter
        /// true - load only "Published" products
        /// false - load only "Unpublished" products
        /// </param>
        /// <returns>Products</returns>
        Task<(IPagedList<Product> products, IList<string> filterableSpecificationAttributeOptionIds)> SearchProducts(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            IList<string> categoryIds = null,
            bool markedAsNewOnly = false,
            bool? featuredProducts = null,
            string productTag = "",
            string keywords = null,
            bool searchDescriptions = false,
            bool searchProductTags = false, 
            string languageId = "",
            ProductSortingEnum orderBy = ProductSortingEnum.Position
        );

        /// <summary>
        /// Update Interval properties
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <param name="Interval">Interval</param>
        /// <param name="IntervalUnit">Interval unit</param>
        /// <param name="includeBothDates">Include both dates</param>
        Task UpdateIntervalProperties(string productId, int interval, IntervalUnit intervalUnit, bool  includeBothDates);


        #endregion

        #region Product Tags

        /// <summary>
        /// Inserts a product tags
        /// </summary>
        /// <param name="productTag">Product Tag</param>
        Task InsertProductTag(ProductTag productTag);

        /// <summary>
        /// Delete a product tags
        /// </summary>
        /// <param name="productTag">Product Tag</param>
        Task DeleteProductTag(ProductTag productTag);

        #endregion

        #region Product pictures

        /// <summary>
        /// Deletes a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        Task DeleteProductPicture(ProductPicture productPicture);

        /// <summary>
        /// Inserts a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        Task InsertProductPicture(ProductPicture productPicture);

        /// <summary>
        /// Updates a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        Task UpdateProductPicture(ProductPicture productPicture);

        #endregion
    }
}
