using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Plugins
{
    public partial class MiscPluginModel : BaseForeverNoteModel
    {
        public string FriendlyName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Misc.Fields.Configure")]
        public string ConfigurationUrl { get; set; }
    }
}