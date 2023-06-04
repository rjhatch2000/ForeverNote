using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Newsletter
{
    public partial class NewsletterBoxModel : BaseForeverNoteModel
    {
        public string NewsletterEmail { get; set; }
        public bool AllowToUnsubscribe { get; set; }
    }
}