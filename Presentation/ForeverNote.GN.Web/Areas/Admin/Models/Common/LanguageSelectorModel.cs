using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Models.Localization;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Common
{
    public partial class LanguageSelectorModel : BaseForeverNoteModel
    {
        public LanguageSelectorModel()
        {
            AvailableLanguages = new List<LanguageModel>();
        }

        public IList<LanguageModel> AvailableLanguages { get; set; }

        public LanguageModel CurrentLanguage { get; set; }
    }
}