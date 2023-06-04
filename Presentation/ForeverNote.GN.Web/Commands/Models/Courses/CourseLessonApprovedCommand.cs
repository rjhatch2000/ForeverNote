using ForeverNote.Core.Domain.Courses;
using ForeverNote.Core.Domain.Customers;
using MediatR;

namespace ForeverNote.Web.Commands.Models.Courses
{
    public class CourseLessonApprovedCommand : IRequest<bool>
    {
        public Course Course { get; set; }
        public CourseLesson Lesson { get; set; }
        public Customer Customer { get; set; }
    }
}
