using ForeverNote.Core.Domain.Courses;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Web.Models.Course;
using MediatR;

namespace ForeverNote.Web.Features.Models.Courses
{
    public class GetCourse : IRequest<CourseModel>
    {
        public Customer Customer { get; set; }
        public Language Language { get; set; }
        public Course Course { get; set; }
    }
}
