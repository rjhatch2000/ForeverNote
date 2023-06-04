using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Messages;

namespace ForeverNote.Web.Areas.Admin.Models.Messages
{
    [Validator(typeof(EmailAccountValidator))]
    public partial class EmailAccountModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Email")]        
        public string Email { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.DisplayName")]        
        public string DisplayName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Host")]        
        public string Host { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Port")]
        public int Port { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Username")]        
        public string Username { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Password")]        
        public string Password { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.UseServerCertificateValidation")]
        public bool UseServerCertificateValidation { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.SecureSocketOptions")]
        public int SecureSocketOptionsId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.IsDefaultEmailAccount")]
        public bool IsDefaultEmailAccount { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.SendTestEmailTo")]        
        public string SendTestEmailTo { get; set; }

    }
}