using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Courses;

namespace ForeverNote.Web.Areas.Admin.Models.Courses
{
    [Validator(typeof(CourseSubjectValidator))]
    public partial class CourseSubjectModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Subject.Fields.Name")]
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Subject.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public string CourseId { get; set; }

    }
}
