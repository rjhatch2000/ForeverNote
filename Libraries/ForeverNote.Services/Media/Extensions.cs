using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Media;
using ForeverNote.Services.Catalog;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Media
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets the download binary array
        /// </summary>
        /// <param name="file">Posted file</param>
        /// <returns>Download binary array</returns>
        public static byte[] GetDownloadBits(this IFormFile file)
        {
            using (var fileStream = file.OpenReadStream())
            using (var ms = new MemoryStream())
            {
                fileStream.CopyTo(ms);
                var fileBytes = ms.ToArray();
                return fileBytes;
            }
        }

        /// <summary>
        /// Gets the picture binary array
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>Picture binary array</returns>
        public static byte[] GetPictureBits(this IFormFile file)
        {
            return GetDownloadBits(file);
        }

        /// <summary>
        /// Get product picture (for shopping cart and order details pages)
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="pictureService">Picture service</param>
        /// <returns>Picture</returns>
        public static async Task<Picture> GetProductPicture(
            this Product product,
            IPictureService pictureService
        )
        {
            if (product == null)
                throw new ArgumentNullException("product");
            if (pictureService == null)
                throw new ArgumentNullException("pictureService");

            Picture picture = null;

            //let's load the default product picture
            if (picture == null)
            {
                var pp = product.ProductPictures.OrderBy(x => x.DisplayOrder).FirstOrDefault();
                if (pp != null)
                    picture = await pictureService.GetPictureById(pp.PictureId);
            }

            return picture;
        }
    }
}
