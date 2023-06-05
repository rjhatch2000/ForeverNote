using ForeverNote.Core;
using ForeverNote.Core.Caching;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Services.Customers;
using ForeverNote.Services.Events;
using ForeverNote.Services.Queries.Models.Catalog;
using ForeverNote.Services.Security;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Catalog
{
    /// <summary>
    /// Product service
    /// </summary>
    public partial class ProductService : IProductService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// </remarks>
        private const string PRODUCTS_BY_ID_KEY = "ForeverNote.product.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTS_PATTERN_KEY = "ForeverNote.product.";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTS_SHOWONHOMEPAGE = "ForeverNote.product.showonhomepage";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer ID
        /// </remarks>
        private const string PRODUCTS_CUSTOMER_ROLE = "ForeverNote.product.cr-{0}";
        private const string PRODUCTS_CUSTOMER_ROLE_PATTERN = "ForeverNote.product.cr";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer ID
        /// </remarks>
        private const string PRODUCTS_CUSTOMER_TAG = "ForeverNote.product.ct-{0}";
        private const string PRODUCTS_CUSTOMER_TAG_PATTERN = "ForeverNote.product.ct";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer ID
        /// </remarks>
        private const string PRODUCTS_CUSTOMER_PERSONAL = "ForeverNote.product.personal-{0}";
        private const string PRODUCTS_CUSTOMER_PERSONAL_PATTERN = "ForeverNote.product.personal";

        #endregion

        #region Fields

        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductTag> _productTagRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerRoleProduct> _customerRoleProductRepository;
        private readonly IRepository<CustomerTagProduct> _customerTagProductRepository;
        private readonly IRepository<CustomerProduct> _customerProductRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly IAclService _aclService;
        private readonly IMediator _mediator;
        private readonly CatalogSettings _catalogSettings;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public ProductService(
            ICacheManager cacheManager,
            IRepository<Product> productRepository,
            IRepository<Customer> customerRepository,
            IRepository<CustomerRoleProduct> customerRoleProductRepository,
            IRepository<CustomerTagProduct> customerTagProductRepository,
            IRepository<CustomerProduct> customerProductRepository,
            IRepository<ProductTag> productTagRepository,
            IWorkContext workContext,
            IMediator mediator,
            IAclService aclService,
            CatalogSettings catalogSettings
        )
        {
            _cacheManager = cacheManager;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _customerRoleProductRepository = customerRoleProductRepository;
            _customerTagProductRepository = customerTagProductRepository;
            _productTagRepository = productTagRepository;
            _customerProductRepository = customerProductRepository;
            _workContext = workContext;
            _mediator = mediator;
            _aclService = aclService;
            _catalogSettings = catalogSettings;
        }

        #endregion

        #region Methods

        #region Products

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="product">Product</param>
        public virtual async Task DeleteProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            //delete customer role product
            var filtersCrp = Builders<CustomerRoleProduct>.Filter;
            var filterCrp = filtersCrp.Eq(x => x.ProductId, product.Id);
            await _customerRoleProductRepository.Collection.DeleteManyAsync(filterCrp);

            //delete product tags
            var existingProductTags = _productTagRepository.Table.Where(x => product.ProductTags.ToList().Contains(x.Name)).ToList();
            foreach (var tag in existingProductTags)
            {
                tag.ProductId = product.Id;
                await DeleteProductTag(tag);
            }

            //deleted product
            await _productRepository.DeleteAsync(product);

            //cache
            await _cacheManager.RemoveByPrefix(PRODUCTS_PATTERN_KEY);

        }

        /// <summary>
        /// Gets all products displayed on the home page
        /// </summary>
        /// <returns>Products</returns>
        public virtual async Task<IList<Product>> GetAllProductsDisplayedOnHomePage()
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Eq(x => x.ShowOnHomePage, true);
            var query = _productRepository.Collection.Find(filter).SortBy(x => x.DisplayOrder).ThenBy(x => x.Name);

            var products = await query.ToListAsync();

            //ACL mapping
            products = products.Where(p => _aclService.Authorize(p)).ToList();

            //availability dates
            products = products.ToList();
            return products;
        }

        /// <summary>
        /// Gets product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="fromDB">get data from db (not from cache)</param>
        /// <returns>Product</returns>
        public virtual async Task<Product> GetProductById(string productId, bool fromDB = false)
        {
            if (string.IsNullOrEmpty(productId))
                return null;

            if (fromDB)
                return await _productRepository.GetByIdAsync(productId);

            var key = string.Format(PRODUCTS_BY_ID_KEY, productId);
            return await _cacheManager.GetAsync(key, () => _productRepository.GetByIdAsync(productId));
        }

        /// <summary>
        /// Gets product for order
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product</returns>
        public virtual async Task<Product> GetProductByIdIncludeArch(string productId)
        {
            if (String.IsNullOrEmpty(productId))
                return null;
            var product = await _productRepository.GetByIdAsync(productId);
            return product;
        }


        /// <summary>
        /// Get products by identifiers
        /// </summary>
        /// <param name="productIds">Product identifiers</param>
        /// <returns>Products</returns>
        public virtual async Task<IList<Product>> GetProductsByIds(string[] productIds, bool showHidden = false)
        {
            if (productIds == null || productIds.Length == 0)
                return new List<Product>();

            var builder = Builders<Product>.Filter;
            var filter = builder.Where(x => productIds.Contains(x.Id));
            var products = await _productRepository.Collection.Find(filter).ToListAsync();

            //sort by passed identifiers
            var sortedProducts = new List<Product>();
            foreach (string id in productIds)
            {
                var product = products.Find(x => x.Id == id);
                if (product != null && (showHidden || (_aclService.Authorize(product))))
                    sortedProducts.Add(product);
            }

            return sortedProducts;
        }

        /// <summary>
        /// Inserts a product
        /// </summary>
        /// <param name="product">Product</param>
        public virtual async Task InsertProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            //insert
            await _productRepository.InsertAsync(product);

            //clear cache
            await _cacheManager.RemoveByPrefix(PRODUCTS_PATTERN_KEY);

            //event notification
            await _mediator.EntityInserted(product);
        }

        /// <summary>
        /// Updates the product
        /// </summary>
        /// <param name="product">Product</param>
        public virtual async Task UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("product");
            var oldProduct = await _productRepository.GetByIdAsync(product.Id);
            //update
            var builder = Builders<Product>.Filter;
            var filter = builder.Eq(x => x.Id, product.Id);
            var update = Builders<Product>.Update
                .Set(x => x.AdminComment, product.AdminComment)
                .Set(x => x.AllowBackInStockSubscriptions, product.AllowBackInStockSubscriptions)
                .Set(x => x.CreatedOnUtc, product.CreatedOnUtc)
                .Set(x => x.CustomerRoles, product.CustomerRoles)
                .Set(x => x.DeliveryDateId, product.DeliveryDateId)
                .Set(x => x.DisplayOrder, product.DisplayOrder)
                .Set(x => x.DisplayOrderCategory, product.DisplayOrderCategory)
                .Set(x => x.Flag, product.Flag)
                .Set(x => x.FullDescription, product.FullDescription)
                .Set(x => x.IncBothDate, product.IncBothDate)
                .Set(x => x.IsRecurring, product.IsRecurring)
                .Set(x => x.Locales, product.Locales)
                .Set(x => x.MarkAsNew, product.MarkAsNew)
                .Set(x => x.MarkAsNewStartDateTimeUtc, product.MarkAsNewStartDateTimeUtc)
                .Set(x => x.MarkAsNewEndDateTimeUtc, product.MarkAsNewEndDateTimeUtc)
                .Set(x => x.MetaDescription, product.MetaDescription)
                .Set(x => x.MetaKeywords, product.MetaKeywords)
                .Set(x => x.MetaTitle, product.MetaTitle)
                .Set(x => x.Name, product.Name)
                .Set(x => x.OnSale, product.OnSale)
                .Set(x => x.RecurringCycleLength, product.RecurringCycleLength)
                .Set(x => x.RecurringCyclePeriodId, product.RecurringCyclePeriodId)
                .Set(x => x.RecurringTotalCycles, product.RecurringTotalCycles)
                .Set(x => x.ShortDescription, product.ShortDescription)
                .Set(x => x.ShowOnHomePage, product.ShowOnHomePage)
                .Set(x => x.SubjectToAcl, product.SubjectToAcl)
                .Set(x => x.UnitId, product.UnitId)
                .Set(x => x.GenericAttributes, product.GenericAttributes)
                .CurrentDate("UpdatedOnUtc");

            await _productRepository.Collection.UpdateOneAsync(filter, update);

            //cache
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, product.Id));
            await _cacheManager.RemoveByPrefix(PRODUCTS_CUSTOMER_PERSONAL_PATTERN);
            await _cacheManager.RemoveByPrefix(PRODUCTS_CUSTOMER_ROLE_PATTERN);
            await _cacheManager.RemoveByPrefix(PRODUCTS_CUSTOMER_TAG_PATTERN);

            //event notification
            await _mediator.EntityUpdated(product);
        }

        public virtual async Task UpdateMostView(string productId, int qty)
        {
            var update = new UpdateDefinitionBuilder<Product>().Inc(x => x.Viewed, qty);
            await _productRepository.Collection.UpdateManyAsync(x => x.Id == productId, update);
        }

        public virtual async Task UpdateSold(string productId, int qty)
        {
            var update = new UpdateDefinitionBuilder<Product>().Inc(x => x.Sold, qty);
            await _productRepository.Collection.UpdateManyAsync(x => x.Id == productId, update);
        }

        /// <summary>
        /// Get (visible) product number in certain category
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="categoryIds">Category identifiers</param>
        /// <returns>Product number</returns>
        public virtual int GetCategoryProductNumber(Customer customer, IList<string> categoryIds = null)
        {
            //validate "categoryIds" parameter
            if (categoryIds != null && categoryIds.Contains(""))
                categoryIds.Remove("");

            var builder = Builders<Product>.Filter;
            var filter = builder.Where(p => true);
            ////category filtering
            if (categoryIds != null && categoryIds.Any())
            {
                filter = filter & builder.Where(p => p.ProductCategories.Any(x => categoryIds.Contains(x.CategoryId)));
            }

            if (!_catalogSettings.IgnoreAcl)
            {
                //ACL (access control list)
                var allowedCustomerRolesIds = customer.GetCustomerRoleIds();
                filter = filter & (builder.AnyIn(x => x.CustomerRoles, allowedCustomerRolesIds) | builder.Where(x => !x.SubjectToAcl));
            }

            return Convert.ToInt32(_productRepository.Collection.Find(filter).CountDocuments());
        }

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
        public virtual async Task<(IPagedList<Product> products, IList<string> filterableSpecificationAttributeOptionIds)> SearchProducts(
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
        )
        {

            var model = await _mediator.Send(new GetSearchProductsQuery() {
                Customer = _workContext.CurrentCustomer,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryIds = categoryIds,
                MarkedAsNewOnly = markedAsNewOnly,
                FeaturedProducts = featuredProducts,
                ProductTag = productTag,
                Keywords = keywords,
                SearchDescriptions = searchDescriptions,
                SearchProductTags = searchProductTags,
                LanguageId = languageId,
                OrderBy = orderBy,
            });

            return model;
        }

        /// <summary>
        /// Update Interval properties
        /// </summary>
        /// <param name="Interval">Interval</param>
        /// <param name="IntervalUnit">Interval unit</param>
        /// <param name="includeBothDates">Include both dates</param>
        public virtual async Task UpdateIntervalProperties(string productId, int interval, IntervalUnit intervalUnit, bool includeBothDates)
        {
            var product = await GetProductById(productId);
            if (product == null)
                throw new ArgumentNullException("product");

            var filter = Builders<Product>.Filter.Eq("Id", product.Id);
            var update = Builders<Product>.Update
                    .Set(x => x.Interval, interval)
                    .Set(x => x.IntervalUnitId, (int)intervalUnit)
                    .Set(x => x.IncBothDate, includeBothDates);

            await _productRepository.Collection.UpdateOneAsync(filter, update);

            //event notification
            await _mediator.EntityUpdated(product);

            //cache
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, product.Id));

        }

        #endregion

        #region Product pictures

        /// <summary>
        /// Deletes a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        public virtual async Task DeleteProductPicture(ProductPicture productPicture)
        {
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");

            var updatebuilder = Builders<Product>.Update;
            var update = updatebuilder.Pull(p => p.ProductPictures, productPicture);
            await _productRepository.Collection.UpdateOneAsync(new BsonDocument("_id", productPicture.ProductId), update);

            //cache
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, productPicture.ProductId));

            //event notification
            await _mediator.EntityDeleted(productPicture);
        }

        /// <summary>
        /// Inserts a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        public virtual async Task InsertProductPicture(ProductPicture productPicture)
        {
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");

            var updatebuilder = Builders<Product>.Update;
            var update = updatebuilder.AddToSet(p => p.ProductPictures, productPicture);
            await _productRepository.Collection.UpdateOneAsync(new BsonDocument("_id", productPicture.ProductId), update);

            //cache
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, productPicture.ProductId));

            //event notification
            await _mediator.EntityInserted(productPicture);
        }

        /// <summary>
        /// Inserts a product tag
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        public virtual async Task InsertProductTag(ProductTag productTag)
        {
            if (productTag == null)
                throw new ArgumentNullException("productTag");

            var updatebuilder = Builders<Product>.Update;
            var update = updatebuilder.AddToSet(p => p.ProductTags, productTag.Name);
            await _productRepository.Collection.UpdateOneAsync(new BsonDocument("_id", productTag.ProductId), update);

            var builder = Builders<ProductTag>.Filter;
            var filter = builder.Eq(x => x.Id, productTag.Id);
            var updateTag = Builders<ProductTag>.Update
                .Inc(x => x.Count, 1);
            await _productTagRepository.Collection.UpdateManyAsync(filter, updateTag);

            //cache
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, productTag.ProductId));

            //event notification
            await _mediator.EntityInserted(productTag);
        }

        public virtual async Task DeleteProductTag(ProductTag productTag)
        {
            if (productTag == null)
                throw new ArgumentNullException("productTag");

            var updatebuilder = Builders<Product>.Update;
            var update = updatebuilder.Pull(p => p.ProductTags, productTag.Name);
            await _productRepository.Collection.UpdateOneAsync(new BsonDocument("_id", productTag.ProductId), update);

            var builder = Builders<ProductTag>.Filter;
            var filter = builder.Eq(x => x.Id, productTag.Id);
            var updateTag = Builders<ProductTag>.Update
                .Inc(x => x.Count, -1);
            await _productTagRepository.Collection.UpdateManyAsync(filter, updateTag);

            //cache
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, productTag.ProductId));

            //event notification
            await _mediator.EntityDeleted(productTag);
        }

        /// <summary>
        /// Updates a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        public virtual async Task UpdateProductPicture(ProductPicture productPicture)
        {
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");

            var builder = Builders<Product>.Filter;
            var filter = builder.Eq(x => x.Id, productPicture.ProductId);
            filter = filter & builder.ElemMatch(x => x.ProductPictures, y => y.Id == productPicture.Id);
            var update = Builders<Product>.Update
                .Set(x => x.ProductPictures.ElementAt(-1).DisplayOrder, productPicture.DisplayOrder)
                .Set(x => x.ProductPictures.ElementAt(-1).MimeType, productPicture.MimeType)
                .Set(x => x.ProductPictures.ElementAt(-1).SeoFilename, productPicture.SeoFilename)
                .Set(x => x.ProductPictures.ElementAt(-1).AltAttribute, productPicture.AltAttribute)
                .Set(x => x.ProductPictures.ElementAt(-1).TitleAttribute, productPicture.TitleAttribute);

            await _productRepository.Collection.UpdateManyAsync(filter, update);

            //cache
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, productPicture.ProductId));

            //event notification
            await _mediator.EntityUpdated(productPicture);
        }

        #endregion

        #region Recommended products

        /// <summary>
        /// Gets recommended products for customer roles
        /// </summary>
        /// <param name="customerRoleIds">Customer role ids</param>
        /// <returns>Products</returns>
        public virtual async Task<IList<Product>> GetRecommendedProducts(string[] customerRoleIds)
        {
            return await _cacheManager.GetAsync(string.Format(PRODUCTS_CUSTOMER_ROLE, string.Join(",", customerRoleIds)), async () =>
            {
                var query = from cr in _customerRoleProductRepository.Table
                            where customerRoleIds.Contains(cr.CustomerRoleId)
                            orderby cr.DisplayOrder
                            select cr.ProductId;

                var productIds = await query.ToListAsync();

                var products = new List<Product>();
                var ids = await GetProductsByIds(productIds.Distinct().ToArray());
                foreach (var product in ids)
                    products.Add(product);

                return products;
            });
        }

        #endregion

        #region Suggested products

        /// <summary>
        /// Gets suggested products for customer tags
        /// </summary>
        /// <param name="customerTagIds">Customer role ids</param>
        /// <returns>Products</returns>
        public virtual async Task<IList<Product>> GetSuggestedProducts(string[] customerTagIds)
        {
            return await _cacheManager.GetAsync(string.Format(PRODUCTS_CUSTOMER_TAG, string.Join(",", customerTagIds)), async () =>
            {
                var query = from cr in _customerTagProductRepository.Table
                            where customerTagIds.Contains(cr.CustomerTagId)
                            orderby cr.DisplayOrder
                            select cr.ProductId;

                var productIds = await query.ToListAsync();

                var products = new List<Product>();
                var ids = await GetProductsByIds(productIds.Distinct().ToArray());
                foreach (var product in ids)
                    products.Add(product);

                return products;
            });
        }

        #endregion

        #region Personalized products

        /// <summary>
        /// Gets personalized products for customer 
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        /// <returns>Products</returns>
        public virtual async Task<IList<Product>> GetPersonalizedProducts(string customerId)
        {
            return await _cacheManager.GetAsync(string.Format(PRODUCTS_CUSTOMER_PERSONAL, customerId), async () =>
            {
                var query = from cr in _customerProductRepository.Table
                            where cr.CustomerId == customerId
                            orderby cr.DisplayOrder
                            select cr.ProductId;

                var productIds = await query.Take(_catalogSettings.PersonalizedProductsNumber).ToListAsync();

                var products = new List<Product>();
                var ids = await GetProductsByIds(productIds.Distinct().ToArray());
                foreach (var product in ids)
                    products.Add(product);

                return products;
            });
        }

        #endregion

        #endregion
    }
}
