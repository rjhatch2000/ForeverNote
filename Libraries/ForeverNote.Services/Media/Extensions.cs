using ForeverNote.Core.Domain.Media;
using ForeverNote.Core.Domain.Notes;
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
            using var fileStream = file.OpenReadStream();
            using var ms = new MemoryStream();
            fileStream.CopyTo(ms);
            var fileBytes = ms.ToArray();
            return fileBytes;
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
        /// Get note picture (for shopping cart and order details pages)
        /// </summary>
        /// <param name="note">Note</param>
        /// <param name="pictureService">Picture service</param>
        /// <returns>Picture</returns>
        public static async Task<Picture> GetNotePicture(
            this Note note,
            IPictureService pictureService
        )
        {
            if (note == null)
                throw new ArgumentNullException("note");
            if (pictureService == null)
                throw new ArgumentNullException("pictureService");

            Picture picture = null;

            //let's load the default note picture
            if (picture == null)
            {
                var pp = note.NotePictures.OrderBy(x => x.DisplayOrder).FirstOrDefault();
                if (pp != null)
                    picture = await pictureService.GetPictureById(pp.PictureId);
            }

            return picture;
        }
    }
}
