using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Validators.Customer;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Models.Customer
{
    [Validator(typeof(LoginValidator))]
    public partial class LoginModel : BaseForeverNoteModel
    {
        public bool CheckoutAsGuest { get; set; }

        [ForeverNoteResourceDisplayName("Account.Login.Fields.Email")]
        public string Email { get; set; }

        public bool UsernamesEnabled { get; set; }
        [ForeverNoteResourceDisplayName("Account.Login.Fields.UserName")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [ForeverNoteResourceDisplayName("Account.Login.Fields.Password")]
        public string Password { get; set; }

        [ForeverNoteResourceDisplayName("Account.Login.Fields.RememberMe")]
        public bool RememberMe { get; set; }

        public bool DisplayCaptcha { get; set; }

    }
}