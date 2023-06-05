using ForeverNote.Core.Domain.Catalog;
using System;
using System.Linq;

namespace ForeverNote.Services.Catalog
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class ProductExtensions
    {
        /// <summary>
        /// Indicates whether a product tag exists
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="productTagId">Product tag identifier</param>
        /// <returns>Result</returns>
        public static bool ProductTagExists(this Product product,
            string productTagName)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            bool result = product.ProductTags.FirstOrDefault(pt => pt == productTagName) != null;
            return result;
        }
    }
}
