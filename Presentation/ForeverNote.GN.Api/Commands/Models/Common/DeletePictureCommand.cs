using ForeverNote.Api.DTOs.Common;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Common
{
    public class DeletePictureCommand : IRequest<bool>
    {
        public PictureDto PictureDto { get; set; }
    }
}
