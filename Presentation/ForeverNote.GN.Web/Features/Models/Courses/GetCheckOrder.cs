using ForeverNote.Core.Domain.Courses;
using ForeverNote.Core.Domain.Customers;
using MediatR;

namespace ForeverNote.Web.Features.Models.Courses
{
    public class GetCheckOrder : IRequest<bool>
    {
        public Customer Customer { get; set; }
        public Course Course { get; set; }
    }
}
