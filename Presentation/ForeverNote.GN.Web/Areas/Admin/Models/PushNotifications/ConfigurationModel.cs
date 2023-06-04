using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.PushNotifications
{
    public class ConfigurationModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.PushNotifications.NotificationsEnabled")]
        public bool Enabled { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.PushNotifications.PrivateApiKey")]
        public string PrivateApiKey { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.PushNotifications.PushApiKey")]
        public string PushApiKey { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.PushNotifications.SenderId")]
        public string SenderId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.PushNotifications.AuthDomain")]
        public string AuthDomain { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.PushNotifications.DatabaseUrl")]
        public string DatabaseUrl { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.PushNotifications.ProjectId")]
        public string ProjectId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.PushNotifications.StorageBucket")]
        public string StorageBucket { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.PushNotifications.AllowGuestNotifications")]
        public bool AllowGuestNotifications { get; set; }
    }
}
