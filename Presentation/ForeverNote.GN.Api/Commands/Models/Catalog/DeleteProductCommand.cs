using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public ProductDto Model { get; set; }
    }
}
