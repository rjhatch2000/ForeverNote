using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Courses;

namespace ForeverNote.Web.Areas.Admin.Models.Courses
{
    [Validator(typeof(CourseLevelValidator))]
    public partial class CourseLevelModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.Courses.Level.Fields.Name")]
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Level.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

    }
}
