using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Courses
{
    [Validator(typeof(CourseLessonValidator))]
    public partial class CourseLessonModel : BaseForeverNoteEntityModel
    {
        public CourseLessonModel()
        {
            AvailableSubjects = new List<SelectListItem>();
        }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Lesson.Fields.Name")]
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Lesson.Fields.ShortDescription")]
        public string ShortDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Lesson.Fields.Description")]
        public string Description { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Lesson.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public string CourseId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Lesson.Fields.SubjectId")]
        public string SubjectId { get; set; }
        public IList<SelectListItem> AvailableSubjects { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Lesson.Fields.VideoFile")]
        [UIHint("Download")]
        public string VideoFile { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Lesson.Fields.AttachmentId")]
        [UIHint("Download")]
        public string AttachmentId { get; set; }

        [UIHint("Picture")]
        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Lesson.Fields.PictureId")]
        public string PictureId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Lesson.Fields.Published")]
        public bool Published { get; set; }
        

    }
}
