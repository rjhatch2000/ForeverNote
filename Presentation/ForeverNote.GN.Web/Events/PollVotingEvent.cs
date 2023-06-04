using ForeverNote.Core.Domain.Polls;
using MediatR;

namespace ForeverNote.Web.Events
{
    public class PollVotingEvent : INotification
    {
        public Poll Poll { get; private set; }
        public PollAnswer PollAnswer { get; private set; }
        public PollVotingEvent(Poll poll, PollAnswer pollAnswer)
        {
            Poll = poll;
            PollAnswer = pollAnswer;
        }
    }
}
