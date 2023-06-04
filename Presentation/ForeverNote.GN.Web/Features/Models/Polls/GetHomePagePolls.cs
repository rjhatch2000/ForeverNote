using ForeverNote.Web.Models.Polls;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Web.Features.Models.Polls
{
    public class GetHomePagePolls : IRequest<IList<PollModel>>
    {
    }
}
