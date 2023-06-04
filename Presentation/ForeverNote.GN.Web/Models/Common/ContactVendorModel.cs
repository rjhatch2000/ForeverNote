using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Validators.Common;

namespace ForeverNote.Web.Models.Common
{
    [Validator(typeof(ContactVendorValidator))]
    public partial class ContactVendorModel : BaseForeverNoteModel
    {
        public string VendorId { get; set; }
        public string VendorName { get; set; }

        [ForeverNoteResourceDisplayName("ContactVendor.Email")]
        public string Email { get; set; }

        [ForeverNoteResourceDisplayName("ContactVendor.Subject")]
        public string Subject { get; set; }
        public bool SubjectEnabled { get; set; }

        [ForeverNoteResourceDisplayName("ContactVendor.Enquiry")]
        public string Enquiry { get; set; }

        [ForeverNoteResourceDisplayName("ContactVendor.FullName")]
        public string FullName { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}