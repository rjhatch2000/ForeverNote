using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Customers;

namespace ForeverNote.Web.Areas.Admin.Models.Customers
{
    [Validator(typeof(UserApiValidator))]
    public partial class UserApiModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.System.UserApi.Email")]

        public string Email { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.UserApi.Password")]

        public string Password { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.UserApi.IsActive")]
        public bool IsActive { get; set; }

    }
}
