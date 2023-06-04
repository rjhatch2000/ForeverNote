using ForeverNote.Core.Domain.Messages;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Common
{
    public class PopupModel : BaseForeverNoteEntityModel
    {
        public string Name { get; set; }
        public string Body { get; set; }
        public string CustomerActionId { get; set; }
        public PopupType PopupType { get; set; }
    }
}
