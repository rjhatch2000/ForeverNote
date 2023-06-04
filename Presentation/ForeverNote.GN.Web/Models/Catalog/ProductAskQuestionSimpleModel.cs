using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Validators.Catalog;

namespace ForeverNote.Web.Models.Catalog
{
    [Validator(typeof(ProductAskQuestionSimpleValidator))]
    public partial class ProductAskQuestionSimpleModel: BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Products.AskQuestion.Email")]
        public string AskQuestionEmail { get; set; }

        [ForeverNoteResourceDisplayName("Products.AskQuestion.FullName")]
        public string AskQuestionFullName { get; set; }

        [ForeverNoteResourceDisplayName("Products.AskQuestion.Phone")]
        public string AskQuestionPhone { get; set; }

        [ForeverNoteResourceDisplayName("Products.AskQuestion.Message")]
        public string AskQuestionMessage { get; set; }

        public bool DisplayCaptcha { get; set; }

    }
}