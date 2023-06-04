using ForeverNote.Web.Models.Boards;
using MediatR;

namespace ForeverNote.Web.Features.Models.Boards
{
    public class GetActiveDiscussions : IRequest<ActiveDiscussionsModel>
    {
    }
}
