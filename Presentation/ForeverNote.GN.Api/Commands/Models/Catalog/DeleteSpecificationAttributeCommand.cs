using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class DeleteSpecificationAttributeCommand : IRequest<bool>
    {
        public SpecificationAttributeDto Model { get; set; }
    }
}
