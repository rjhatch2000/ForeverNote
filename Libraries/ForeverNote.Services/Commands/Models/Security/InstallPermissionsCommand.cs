using ForeverNote.Services.Security;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Security
{
    public class InstallPermissionsCommand : IRequest<bool>
    {
        public IPermissionProvider PermissionProvider { get; set; }
    }
}
