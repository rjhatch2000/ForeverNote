using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Validators.Customer;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Models.Customer
{
    [Validator(typeof(DeleteAccountValidator))]
    public partial class DeleteAccountModel : BaseForeverNoteModel
    {
        [DataType(DataType.Password)]
        [ForeverNoteResourceDisplayName("Account.DeleteAccount.Fields.Password")]
        public string Password { get; set; }

    }
}