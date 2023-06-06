using ForeverNote.Core.Domain.Users;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Services.Commands.Models.Users
{
    public class UserActionEventReactionCommand : IRequest<bool>
    {
        public UserActionEventReactionCommand()
        {
            UserActionTypes = new List<UserActionType>();
        }
        public IList<UserActionType> UserActionTypes { get; set; }
        public UserAction Action { get; set; }
        public string UserId { get; set; }
    }
}
