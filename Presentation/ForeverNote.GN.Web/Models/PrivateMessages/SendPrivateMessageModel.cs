using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Validators.PrivateMessages;

namespace ForeverNote.Web.Models.PrivateMessages
{
    [Validator(typeof(SendPrivateMessageValidator))]
    public partial class SendPrivateMessageModel : BaseForeverNoteEntityModel
    {
        public string ToCustomerId { get; set; }
        public string CustomerToName { get; set; }
        public bool AllowViewingToProfile { get; set; }

        public string ReplyToMessageId { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}