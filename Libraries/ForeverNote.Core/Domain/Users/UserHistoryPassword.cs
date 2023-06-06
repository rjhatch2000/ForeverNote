namespace ForeverNote.Core.Domain.Users
{
    /// <summary>
    /// Represents a User History Password
    /// </summary>
    public partial class UserHistoryPassword : BaseEntity
    {
        public UserHistoryPassword()
        {
            this.PasswordFormat = PasswordFormat.Clear;
        }

        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password format identifier
        /// </summary>
        public int PasswordFormatId { get; set; }

        /// <summary>
        /// Gets or sets the password salt
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Gets or sets the password format
        /// </summary>
        public PasswordFormat PasswordFormat
        {
            get { return (PasswordFormat)PasswordFormatId; }
            set { this.PasswordFormatId = (int)value; }
        }
    }
}
