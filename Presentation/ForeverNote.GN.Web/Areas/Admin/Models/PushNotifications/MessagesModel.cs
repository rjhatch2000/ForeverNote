using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.PushNotifications
{
    public class MessagesModel : BaseForeverNoteModel
    {
        public int Allowed { get; set; }

        public int Denied { get; set; }
    }
}
