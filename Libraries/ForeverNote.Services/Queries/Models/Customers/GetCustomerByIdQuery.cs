using ForeverNote.Core.Domain.Customers;
using MediatR;
namespace ForeverNote.Services.Queries.Models.Customers
{
    public class GetCustomerByIdQuery : IRequest<Customer>
    {
        public string Id { get; set; }
    }
}
