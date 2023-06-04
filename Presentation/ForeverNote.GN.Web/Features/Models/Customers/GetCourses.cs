using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Customer;
using MediatR;

namespace ForeverNote.Web.Features.Models.Customers
{
    public class GetCourses : IRequest<CoursesModel>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
    }
}
