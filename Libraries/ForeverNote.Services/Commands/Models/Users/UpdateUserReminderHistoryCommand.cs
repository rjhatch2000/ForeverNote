using MediatR;

namespace ForeverNote.Services.Commands.Models.Users
{
    public class UpdateUserReminderHistoryCommand : IRequest<bool>
    {
        public string UserId { get; set; }
    }
}
