using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Templates;

namespace ForeverNote.Web.Areas.Admin.Models.Templates
{
    [Validator(typeof(ProductTemplateValidator))]
    public partial class ProductTemplateModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.System.Templates.Product.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.Templates.Product.ViewPath")]
        
        public string ViewPath { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.Templates.Product.DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
}