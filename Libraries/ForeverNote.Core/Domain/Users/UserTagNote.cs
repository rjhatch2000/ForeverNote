namespace ForeverNote.Core.Domain.Users
{
   
    /// <summary>
    /// Represents a user tag note
    /// </summary>
    public partial class UserTagNote : BaseEntity
    {

        /// <summary>
        /// Gets or sets the user tag id
        /// </summary>
        public string UserTagId { get; set; }

        /// <summary>
        /// Gets or sets the note Id
        /// </summary>
        public string NoteId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

    }

}