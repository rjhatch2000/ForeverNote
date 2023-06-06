using System;

namespace ForeverNote.Core.Domain.PushNotifications
{
    public class PushRegistration : BaseEntity
    {
        public string UserId { get; set; }

        public bool Allowed { get; set; }

        public string Token { get; set; }

        public DateTime RegisteredOn { get; set; }
    }
}
