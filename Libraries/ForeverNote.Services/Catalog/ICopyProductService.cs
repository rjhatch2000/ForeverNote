using ForeverNote.Core.Domain.Catalog;
using System.Threading.Tasks;

namespace ForeverNote.Services.Catalog
{
    /// <summary>
    /// Copy product service
    /// </summary>
    public partial interface ICopyProductService
    {
        /// <summary>
        /// Create a copy of product with all depended data
        /// </summary>
        /// <param name="product">The product to copy</param>
        /// <param name="newName">The name of product duplicate</param>
        /// <param name="copyImages">A value indicating whether the product images should be copied</param>
        /// <param name="copyAssociatedProducts">A value indicating whether the copy associated products</param>
        /// <returns>Product copy</returns>
        Task<Product> CopyProduct(Product product, string newName,
            bool copyImages = true, bool copyAssociatedProducts = true);
    }
}
