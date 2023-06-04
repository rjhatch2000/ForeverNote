using ForeverNote.Api.DTOs.Common;
using MediatR;

namespace ForeverNote.Api.Queries.Models.Common
{
    public class GetPictureByIdQuery : IRequest<PictureDto>
    {
        public string Id { get; set; }
    }
}
