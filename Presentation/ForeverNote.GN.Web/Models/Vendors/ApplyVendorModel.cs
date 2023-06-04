using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Validators.Vendors;

namespace ForeverNote.Web.Models.Vendors
{
    [Validator(typeof(ApplyVendorValidator))]
    public partial class ApplyVendorModel : BaseForeverNoteModel
    {

        public ApplyVendorModel()
        {
            Address = new VendorAddressModel();
        }

        public VendorAddressModel Address { get; set; }

        [ForeverNoteResourceDisplayName("Vendors.ApplyAccount.Name")]
        public string Name { get; set; }
        [ForeverNoteResourceDisplayName("Vendors.ApplyAccount.Email")]
        public string Email { get; set; }
        [ForeverNoteResourceDisplayName("Vendors.ApplyAccount.Description")]
        public string Description { get; set; }
        public bool DisplayCaptcha { get; set; }
        public bool TermsOfServiceEnabled { get; set; }
        public bool TermsOfServicePopup { get; set; }
        public bool DisableFormInput { get; set; }
        public string Result { get; set; }
    }
}