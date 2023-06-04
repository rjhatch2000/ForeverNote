using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Messages;
using System;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Messages
{
    [Validator(typeof(QueuedEmailValidator))]
    public partial class QueuedEmailModel: BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.Id")]
        public override string Id { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.Priority")]
        public string PriorityName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.From")]
        
        public string From { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.FromName")]
        
        public string FromName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.To")]
        
        public string To { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.ToName")]
        
        public string ToName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.ReplyTo")]
        
        public string ReplyTo { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.ReplyToName")]
        
        public string ReplyToName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.CC")]
        
        public string CC { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.Bcc")]
        
        public string Bcc { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.Subject")]
        
        public string Subject { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.Body")]
        
        public string Body { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.AttachmentFilePath")]
        
        public string AttachmentFilePath { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.AttachedDownload")]
        [UIHint("Download")]
        public string AttachedDownloadId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.SendImmediately")]
        public bool SendImmediately { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.DontSendBeforeDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? DontSendBeforeDate { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.SentTries")]
        public int SentTries { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.SentOn")]
        public DateTime? SentOn { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.Fields.EmailAccountName")]
        
        public string EmailAccountName { get; set; }
    }
}