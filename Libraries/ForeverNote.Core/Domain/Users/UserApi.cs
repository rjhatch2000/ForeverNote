﻿namespace ForeverNote.Core.Domain.Users
{
    public partial class UserApi : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PrivateKey { get; set; }
        public bool IsActive { get; set; }
    }
}
