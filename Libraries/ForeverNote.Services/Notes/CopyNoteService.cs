using ForeverNote.Core.Domain.Notes;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Media;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Notes
{
    /// <summary>
    /// Copy Note service
    /// </summary>
    public partial class CopyNoteService : ICopyNoteService
    {
        #region Fields

        private readonly INoteService _noteService;
        private readonly ILanguageService _languageService;
        private readonly IPictureService _pictureService;
        private readonly IDownloadService _downloadService;
        #endregion

        #region Ctor

        public CopyNoteService(INoteService noteService,
            ILanguageService languageService,
            IPictureService pictureService,
            IDownloadService downloadService
        )
        {
            _noteService = noteService;
            _languageService = languageService;
            _pictureService = pictureService;
            _downloadService = downloadService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a copy of note with all depended data
        /// </summary>
        /// <param name="note">The note to copy</param>
        /// <param name="newName">The name of note duplicate</param>
        /// <param name="isPublished">A value indicating whether the note duplicate should be published</param>
        /// <param name="copyImages">A value indicating whether the note images should be copied</param>
        /// <param name="copyAssociatedNotes">A value indicating whether the copy associated notes</param>
        /// <returns>Note copy</returns>
        public virtual async Task<Note> CopyNote(Note note, string newName,
            bool copyImages = true, bool copyAssociatedNotes = true)
        {
            if (note == null)
                throw new ArgumentNullException("note");

            if (string.IsNullOrEmpty(newName))
                throw new ArgumentException("Note name is required");

            // note
            var noteCopy = new Note
            {
                Name = newName,
                ShortDescription = note.ShortDescription,
                FullDescription = note.FullDescription,
                Flag = note.Flag,
                AdminComment = note.AdminComment,
                ShowOnHomePage = note.ShowOnHomePage,
                IsRecurring = note.IsRecurring,
                RecurringCycleLength = note.RecurringCycleLength,
                RecurringCyclePeriod = note.RecurringCyclePeriod,
                RecurringTotalCycles = note.RecurringTotalCycles,
                DeliveryDateId = note.DeliveryDateId,
                AllowBackInStockSubscriptions = note.AllowBackInStockSubscriptions,
                MarkAsNew = note.MarkAsNew,
                MarkAsNewStartDateTimeUtc = note.MarkAsNewStartDateTimeUtc,
                MarkAsNewEndDateTimeUtc = note.MarkAsNewEndDateTimeUtc,
                DisplayOrder = note.DisplayOrder,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
                Locales = note.Locales,
                UserRoles = note.UserRoles,
            };

            // note <-> notebooks mappings
            foreach (var noteNotebook in note.NoteNotebooks)
            {
                noteCopy.NoteNotebooks.Add(noteNotebook);
            }

            //note tags
            foreach (var noteTag in note.NoteTags)
            {
                noteCopy.NoteTags.Add(noteTag);
            }

            //validate search engine name
            await _noteService.InsertNote(noteCopy);

            var languages = await _languageService.GetAllLanguages(true);

            //note pictures
            //variable to store original and new picture identifiers
            int id = 1;
            var originalNewPictureIdentifiers = new Dictionary<string, string>();
            if (copyImages)
            {
                foreach (var notePicture in note.NotePictures)
                {
                    var picture = await _pictureService.GetPictureById(notePicture.PictureId);
                    var pictureCopy = await _pictureService.InsertPicture(
                        await _pictureService.LoadPictureBinary(picture),
                        picture.MimeType,
                        newName,
                        picture.AltAttribute,
                        picture.TitleAttribute);

                    await _noteService.InsertNotePicture(new NotePicture
                    {
                        NoteId = noteCopy.Id,
                        PictureId = pictureCopy.Id,
                        DisplayOrder = notePicture.DisplayOrder
                    });
                    id++;
                    originalNewPictureIdentifiers.Add(picture.Id, pictureCopy.Id);
                }
            }


            return noteCopy;
        }

        #endregion
    }
}