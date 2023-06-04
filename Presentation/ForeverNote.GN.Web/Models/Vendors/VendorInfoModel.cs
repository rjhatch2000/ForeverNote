using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Validators.Vendors;

namespace ForeverNote.Web.Models.Vendors
{
    [Validator(typeof(VendorInfoValidator))]
    public partial class VendorInfoModel : BaseForeverNoteModel
    {
        public VendorInfoModel()
        {
            Address = new VendorAddressModel();
        }

        [ForeverNoteResourceDisplayName("Account.VendorInfo.Name")]
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Account.VendorInfo.Email")]
        public string Email { get; set; }

        [ForeverNoteResourceDisplayName("Account.VendorInfo.Description")]
        public string Description { get; set; }

        [ForeverNoteResourceDisplayName("Account.VendorInfo.Picture")]
        public string PictureUrl { get; set; }

        public VendorAddressModel Address { get; set; }
    }
}