using FluentValidation.Attributes;
using ForeverNote.Web.Areas.Api.Validators.Common;

namespace ForeverNote.Web.Areas.Api.Models.Common
{
    [Validator(typeof(LoginValidator))]
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
