using ForeverNote.Core.Domain.Notes;
using System.Threading.Tasks;

namespace ForeverNote.Services.Notes
{
    /// <summary>
    /// Copy note service
    /// </summary>
    public partial interface ICopyNoteService
    {
        /// <summary>
        /// Create a copy of note with all depended data
        /// </summary>
        /// <param name="note">The note to copy</param>
        /// <param name="newName">The name of note duplicate</param>
        /// <param name="copyImages">A value indicating whether the note images should be copied</param>
        /// <param name="copyAssociatedNotes">A value indicating whether the copy associated notes</param>
        /// <returns>Note copy</returns>
        Task<Note> CopyNote(Note note, string newName,
            bool copyImages = true, bool copyAssociatedNotes = true);
    }
}
