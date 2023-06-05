using ForeverNote.Core.Domain.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Catalog
{
    /// <summary>
    /// Product tag service interface
    /// </summary>
    public partial interface IProductTagService
    {
        /// <summary>
        /// Delete a product tag
        /// </summary>
        /// <param name="productTag">Product tag</param>
        Task DeleteProductTag(ProductTag productTag);

        /// <summary>
        /// Gets all product tags
        /// </summary>
        /// <returns>Product tags</returns>
        Task<IList<ProductTag>> GetAllProductTags();

        /// <summary>
        /// Gets product tag
        /// </summary>
        /// <param name="productTagId">Product tag identifier</param>
        /// <returns>Product tag</returns>
        Task<ProductTag> GetProductTagById(string productTagId);

        /// <summary>
        /// Gets product tag by name
        /// </summary>
        /// <param name="name">Product tag name</param>
        /// <returns>Product tag</returns>
        Task<ProductTag> GetProductTagByName(string name);

        /// <summary>
        /// Inserts a product tag
        /// </summary>
        /// <param name="productTag">Product tag</param>
        Task InsertProductTag(ProductTag productTag);

        /// <summary>
        /// Update a product tag
        /// </summary>
        /// <param name="productTag">Product tag</param>
        Task UpdateProductTag(ProductTag productTag);

        /// <summary>
        /// Get number of products
        /// </summary>
        /// <param name="productTagId">Product tag identifier</param>
        /// <returns>Number of products</returns>
        Task<int> GetProductCount(string productTagId);
    }
}
