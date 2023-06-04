using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Topics
{
    public partial class TopicModel : BaseForeverNoteEntityModel
    {
        public string SystemName { get; set; }

        public bool IncludeInSitemap { get; set; }

        public bool IsPasswordProtected { get; set; }

        public string Password { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public string MetaTitle { get; set; }

        public string SeName { get; set; }

        public string TopicTemplateId { get; set; }

        public bool Published { get; set; }
    }
}