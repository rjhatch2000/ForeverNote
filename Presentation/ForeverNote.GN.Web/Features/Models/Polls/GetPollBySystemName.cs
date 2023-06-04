using ForeverNote.Web.Models.Polls;
using MediatR;

namespace ForeverNote.Web.Features.Models.Polls
{
    public class GetPollBySystemName : IRequest<PollModel>
    {
        public string SystemName { get; set; }
    }
}
