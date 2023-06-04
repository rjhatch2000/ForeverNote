using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Documents
{
    public class DocumentListModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.Documents.Document.List.SearchName")]
        public string SearchName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.List.SearchNumber")]
        public string SearchNumber { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.List.SearchEmail")]
        public string SearchEmail { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.List.DocumentStatus")]
        public int StatusId { get; set; }

        public int Reference { get; set; }

        public string ObjectId { get; set; }
        public string CustomerId { get; set; }

    }
}
