using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Common
{
    public partial class LogoModel : BaseForeverNoteModel
    {
        public string StoreName { get; set; }

        public string LogoPath { get; set; }
    }
}