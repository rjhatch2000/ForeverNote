using System;

namespace ForeverNote.Core.Domain.Users
{
    /// <summary>
    /// Represents a user action
    /// </summary>
    public partial class UserActionHistory : BaseEntity
    {
        public string UserActionId { get; set; }
        public string UserId { get; set; }
        public DateTime CreateDateUtc { get; set; }

    }
}
