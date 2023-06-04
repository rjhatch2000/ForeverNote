using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Common
{
    public partial class LanguageModel : BaseForeverNoteEntityModel
    {
        public string Name { get; set; }

        public string FlagImageFileName { get; set; }

    }
}