using ForeverNote.Core.Domain.Vendors;
using MediatR;
namespace ForeverNote.Services.Queries.Models.Customers
{
    public class GetVendorByIdQuery : IRequest<Vendor>
    {
        public string Id { get; set; }
    }
}
