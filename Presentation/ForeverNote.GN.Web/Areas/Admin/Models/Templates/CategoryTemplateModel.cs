using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Templates;

namespace ForeverNote.Web.Areas.Admin.Models.Templates
{
    [Validator(typeof(CategoryTemplateValidator))]
    public partial class CategoryTemplateModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.System.Templates.Category.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.Templates.Category.ViewPath")]
        
        public string ViewPath { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.Templates.Category.DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
}