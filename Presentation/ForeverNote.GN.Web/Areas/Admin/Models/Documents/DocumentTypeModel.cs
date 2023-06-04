using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Documents;

namespace ForeverNote.Web.Areas.Admin.Models.Documents
{
    [Validator(typeof(DocumentTypeValidator))]
    public partial class DocumentTypeModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.Documents.Type.Fields.Name")]

        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Type.Fields.Description")]

        public string Description { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Type.Fields.DisplayOrder")]

        public int DisplayOrder { get; set; }
    }
}
