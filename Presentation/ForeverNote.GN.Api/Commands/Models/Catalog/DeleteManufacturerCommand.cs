using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class DeleteManufacturerCommand : IRequest<bool>
    {
        public ManufacturerDto Model { get; set; }
    }
}
