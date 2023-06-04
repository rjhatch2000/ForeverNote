using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class AddManufacturerCommand : IRequest<ManufacturerDto>
    {
        public ManufacturerDto Model { get; set; }
    }
}
