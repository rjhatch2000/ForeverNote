using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Common
{
    public partial class UrlRecordModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.System.SeNames.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.SeNames.EntityId")]
        public string EntityId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.SeNames.EntityName")]
        public string EntityName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.SeNames.IsActive")]
        public bool IsActive { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.SeNames.Language")]
        public string Language { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.SeNames.Details")]
        public string DetailsUrl { get; set; }
    }
}