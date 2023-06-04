using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Localization
{
    public partial class LanguageResourceFilterModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.Configuration.Languages.ResourcesFilter.Fields.ResourceName")]
        public string ResourceName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Languages.ResourcesFilter.Fields.ResourceValue")]
        public string ResourceValue { get; set; }

    }
}