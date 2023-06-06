namespace ForeverNote.Core.Domain.Notes
{
    /// <summary>
    /// Represents a note picture mapping
    /// </summary>
    public partial class NotePicture : SubBaseEntity
    {
        /// <summary>
        /// Gets or sets the note identifier
        /// </summary>
        public string NoteId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public string PictureId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the SEO friednly filename of the picture
        /// </summary>
        public string SeoFilename { get; set; }

        /// <summary>
        /// Gets or sets the "alt" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. note name)
        /// </summary>
        public string AltAttribute { get; set; }

        /// <summary>
        /// Gets or sets the "title" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. note name)
        /// </summary>
        public string TitleAttribute { get; set; }

    }

}
