using MediatR;

namespace ForeverNote.Services.Commands.Models.Orders
{
    public class PrepareOrderCodeCommand : IRequest<string>
    {
    }
}
