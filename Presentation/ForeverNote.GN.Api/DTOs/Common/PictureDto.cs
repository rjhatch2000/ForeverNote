using FluentValidation.Attributes;
using ForeverNote.Api.Validators.Common;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Api.DTOs.Common
{
    [Validator(typeof(PictureValidator))]
    public partial class PictureDto : BaseApiEntityModel
    {
        public byte[] PictureBinary { get; set; }
        public string MimeType { get; set; }
        public string SeoFilename { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }
        public bool IsNew { get; set; }
    }
}
