using ForeverNote.Web.Framework.Mvc.Models;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Course
{
    public partial class CourseModel : BaseForeverNoteEntityModel
    {
        public CourseModel()
        {
            Subjects = new List<Subject>();
            Lessons = new List<Lesson>();
        }

        public string Name { get; set; }
        public string Level { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }
        public string PictureUrl { get; set; }
        public IList<Subject> Subjects { get; set; }
        public IList<Lesson> Lessons { get; set; }
        public bool Approved { get; set; }

        public partial class Subject : BaseForeverNoteEntityModel
        {
            public string Name { get; set; }
            public int DisplayOrder { get; set; }
        }

        public partial class Lesson : BaseForeverNoteEntityModel
        {
            public string SubjectId { get; set; }
            public string Name { get; set; }
            public string ShortDescription { get; set; }
            public int DisplayOrder { get; set; }
            public string PictureUrl { get; set; }
            public bool Approved { get; set; }
        }

    }
}
