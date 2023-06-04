using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Newsletter
{
    public partial class SubscribeNewsletterResultModel: BaseForeverNoteModel
    {
        public string Result { get; set; }
        public string ResultCategory { get; set; }
        public bool Success { get; set; }
        public bool ShowCategories { get; set; }
        public NewsletterCategoryModel NewsletterCategory { get; set; }
    }
}