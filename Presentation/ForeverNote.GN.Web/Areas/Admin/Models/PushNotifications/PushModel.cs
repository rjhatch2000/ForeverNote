using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.PushNotifications
{
    public partial class PushModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.PushNotifications.Fields.PushTitle")]
        public string Title { get; set; }

        [ForeverNoteResourceDisplayName("Admin.PushNotifications.Fields.PushMessageText")]
        public string MessageText { get; set; }

        [UIHint("Picture")]
        [ForeverNoteResourceDisplayName("Admin.PushNotifications.Fields.Picture")]
        public string PictureId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.PushNotifications.Fields.ClickUrl")]
        public string ClickUrl { get; set; }
    }
}
