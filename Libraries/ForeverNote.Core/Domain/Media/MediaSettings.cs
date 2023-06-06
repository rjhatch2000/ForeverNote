using ForeverNote.Core.Configuration;

namespace ForeverNote.Core.Domain.Media
{
    public class MediaSettings : ISettings
    {
        public int AvatarPictureSize { get; set; }
        public int NoteThumbPictureSize { get; set; }
        public int NoteDetailsPictureSize { get; set; }
        public int NoteThumbPictureSizeOnNoteDetailsPage { get; set; }
        public int AssociatedNotePictureSize { get; set; }
        public int NotebookThumbPictureSize { get; set; }
        public int AutoCompleteSearchThumbPictureSize { get; set; }
        public int ImageSquarePictureSize { get; set; }
        public bool DefaultPictureZoomEnabled { get; set; }

        public int MaximumImageSize { get; set; }

        /// <summary>
        /// Geta or sets a default quality used for image generation
        /// </summary>
        public int DefaultImageQuality { get; set; }
        
        /// <summary>
        /// Geta or sets a vaue indicating whether single (/content/images/thumbs/) or multiple (/content/images/thumbs/001/ and /content/images/thumbs/002/) directories will used for picture thumbs
        /// </summary>
        public bool MultipleThumbDirectories { get; set; }

        public string AllowedFileTypes { get; set; }
        public string StoreLocation { get; set; }

        /// <summary>
        /// Gets a value indicating whether the images should be stored in data base.
        /// </summary>
        public bool StoreInDb { get; set; } = true;
    }
}