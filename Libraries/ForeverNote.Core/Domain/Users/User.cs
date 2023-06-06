using System;
using System.Collections.Generic;

namespace ForeverNote.Core.Domain.Users
{
    /// <summary>
    /// Represents a user
    /// </summary>
    public partial class User : BaseEntity
    {
        private ICollection<string> _userTags;

        /// <summary>
        /// Ctor
        /// </summary>
        public User()
        {
            this.UserGuid = Guid.NewGuid();
            this.PasswordFormat = PasswordFormat.Clear;
        }

        /// <summary>
        /// Gets or sets the user Guid
        /// </summary>
        public Guid UserGuid { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Gets or sets the password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password format
        /// </summary>
        public int PasswordFormatId { get; set; }
        
        /// <summary>
        /// Gets or sets the password format
        /// </summary>
        public PasswordFormat PasswordFormat
        {
            get { return (PasswordFormat)PasswordFormatId; }
            set { this.PasswordFormatId = (int)value; }
        }
        
        /// <summary>
        /// Gets or sets the password salt
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user account is system
        /// </summary>
        public bool IsSystemAccount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating number of failed login attempts (wrong password)
        /// </summary>
        public int FailedLoginAttempts { get; set; }

        /// <summary>
        /// Gets or sets the date and time until which a user cannot login (locked out)
        /// </summary>
        public DateTime? CannotLoginUntilDateUtc { get; set; }
        
        /// <summary>
        /// Gets or sets the user system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
        public string LastIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the source Url Referrer
        /// </summary>
        public string UrlReferrer { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last login
        /// </summary>
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
        public DateTime LastActivityDateUtc { get; set; }

        /// <summary>
        /// Last date to change password
        /// </summary>
        public DateTime? PasswordChangeDateUtc { get; set; }

        #region Navigation properties

        /// <summary>
        /// Gets or sets the user tags
        /// </summary>
        public virtual ICollection<string> UserTags
        {
            get { return _userTags ?? (_userTags = new List<string>()); }
            protected set { _userTags = value; }
        }
        #endregion
    }
}