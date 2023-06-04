using ForeverNote.Api.DTOs.Common;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Common
{
    public class AddPictureCommand : IRequest<PictureDto>
    {
        public PictureDto PictureDto { get; set; }
    }
}
