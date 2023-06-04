using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Cms
{
    public partial class WidgetModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Widgets.Fields.FriendlyName")]
        
        public string FriendlyName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Widgets.Fields.SystemName")]
        
        public string SystemName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Widgets.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Widgets.Fields.IsActive")]
        public bool IsActive { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Widgets.Fields.Configure")]
        public string ConfigurationUrl { get; set; }
    }
}