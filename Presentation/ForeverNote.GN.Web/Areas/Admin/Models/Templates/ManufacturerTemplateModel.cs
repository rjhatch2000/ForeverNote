using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Templates;

namespace ForeverNote.Web.Areas.Admin.Models.Templates
{
    [Validator(typeof(ManufacturerTemplateValidator))]
    public partial class ManufacturerTemplateModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.System.Templates.Manufacturer.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.Templates.Manufacturer.ViewPath")]
        
        public string ViewPath { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.Templates.Manufacturer.DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
}