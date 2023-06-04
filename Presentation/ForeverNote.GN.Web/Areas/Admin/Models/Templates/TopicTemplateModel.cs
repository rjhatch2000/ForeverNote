using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Templates;

namespace ForeverNote.Web.Areas.Admin.Models.Templates
{
    [Validator(typeof(TopicTemplateValidator))]
    public partial class TopicTemplateModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.System.Templates.Topic.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.Templates.Topic.ViewPath")]
        
        public string ViewPath { get; set; }

        [ForeverNoteResourceDisplayName("Admin.System.Templates.Topic.DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
}