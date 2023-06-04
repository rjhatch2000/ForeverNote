using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Common
{
    public partial class AdminHeaderLinksModel : BaseForeverNoteModel
    {
        public string ImpersonatedCustomerEmailUsername { get; set; }
        public bool IsCustomerImpersonated { get; set; }
        public bool DisplayAdminLink { get; set; }
        public string EditPageUrl { get; set; }
    }
}