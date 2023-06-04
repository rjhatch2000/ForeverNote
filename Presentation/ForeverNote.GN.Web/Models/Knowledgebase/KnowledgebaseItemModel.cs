using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Knowledgebase
{
    public class KnowledgebaseItemModel : BaseForeverNoteEntityModel
    {
        public string Name { get; set; }
        public string SeName { get; set; }
        public bool IsArticle { get; set; }
        public string FormattedBreadcrumbs { get; set; }
    }
}
