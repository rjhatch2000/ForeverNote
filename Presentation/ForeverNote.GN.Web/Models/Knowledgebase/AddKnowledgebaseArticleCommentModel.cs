using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Knowledgebase
{
    public partial class AddKnowledgebaseArticleCommentModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Knowledgebase.Article.CommentText")]
        public string CommentText { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}
