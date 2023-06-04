using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Validators.Catalog;

namespace ForeverNote.Web.Models.Catalog
{
    [Validator(typeof(ProductAskQuestionValidator))]
    public partial class ProductAskQuestionModel: BaseForeverNoteEntityModel
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductSeName { get; set; }

        [ForeverNoteResourceDisplayName("Products.AskQuestion.Email")]
        public string Email { get; set; }

        [ForeverNoteResourceDisplayName("Products.AskQuestion.FullName")]
        public string FullName { get; set; }

        [ForeverNoteResourceDisplayName("Products.AskQuestion.Phone")]
        public string Phone { get; set; }

        [ForeverNoteResourceDisplayName("Products.AskQuestion.Message")]
        public string Message { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }

        public bool DisplayCaptcha { get; set; }

    }
}