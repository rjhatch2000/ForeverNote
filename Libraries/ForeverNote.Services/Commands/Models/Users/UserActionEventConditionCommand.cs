using ForeverNote.Core.Domain.Users;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Services.Commands.Models.Users
{
    public class UserActionEventConditionCommand : IRequest<bool>
    {
        public UserActionEventConditionCommand()
        {
            UserActionTypes = new List<UserActionType>();
        }
        public IList<UserActionType> UserActionTypes { get; set; }
        public UserAction Action { get; set; }
        public string NoteId { get; set; }
        public string AttributesXml { get; set; }
        public string UserId { get; set; }
        public string CurrentUrl { get; set; }
        public string PreviousUrl { get; set; }
    }
}
