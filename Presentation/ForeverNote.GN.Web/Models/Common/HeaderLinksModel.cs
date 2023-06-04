using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Common
{
    public partial class HeaderLinksModel : BaseForeverNoteModel
    {
        public bool IsAuthenticated { get; set; }
        public string CustomerEmailUsername { get; set; }
        public bool AllowPrivateMessages { get; set; }
        public string UnreadPrivateMessages { get; set; }
        public string AlertMessage { get; set; }
    }
}