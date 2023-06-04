using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.News
{
    public partial class AddNewsCommentModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("News.Comments.CommentTitle")]
        public string CommentTitle { get; set; }

        [ForeverNoteResourceDisplayName("News.Comments.CommentText")]
        public string CommentText { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}