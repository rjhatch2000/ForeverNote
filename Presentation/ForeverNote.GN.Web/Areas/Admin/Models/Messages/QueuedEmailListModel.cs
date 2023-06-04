using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Messages
{
    public partial class QueuedEmailListModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.List.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? SearchStartDate { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.List.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? SearchEndDate { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.List.FromEmail")]
        
        public string SearchFromEmail { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.List.ToEmail")]
        
        public string SearchToEmail { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.List.LoadNotSent")]
        public bool SearchLoadNotSent { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.List.MaxSentTries")]
        public int SearchMaxSentTries { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.QueuedEmails.List.GoDirectlyToNumber")]
        public string GoDirectlyToNumber { get; set; }
    }
}