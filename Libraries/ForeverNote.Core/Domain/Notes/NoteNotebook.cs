namespace ForeverNote.Core.Domain.Notes
{
    /// <summary>
    /// Represents a note notebook mapping
    /// </summary>
    public partial class NoteNotebook : SubBaseEntity
    {

        /// <summary>
        /// Gets or sets the note identifier
        /// </summary>
        public string NoteId { get; set; }
        /// <summary>
        /// Gets or sets the notebook identifier
        /// </summary>
        public string NotebookId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the note is featured
        /// </summary>
        public bool IsFeaturedNote { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

    }

}
