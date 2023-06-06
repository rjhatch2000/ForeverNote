using ForeverNote.Core.Domain.Notes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Notes
{
    /// <summary>
    /// Note tag service interface
    /// </summary>
    public partial interface INoteTagService
    {
        /// <summary>
        /// Delete a note tag
        /// </summary>
        /// <param name="noteTag">Note tag</param>
        Task DeleteNoteTag(NoteTag noteTag);

        /// <summary>
        /// Gets all note tags
        /// </summary>
        /// <returns>Note tags</returns>
        Task<IList<NoteTag>> GetAllNoteTags();

        /// <summary>
        /// Gets note tag
        /// </summary>
        /// <param name="noteTagId">Note tag identifier</param>
        /// <returns>Note tag</returns>
        Task<NoteTag> GetNoteTagById(string noteTagId);

        /// <summary>
        /// Gets note tag by name
        /// </summary>
        /// <param name="name">Note tag name</param>
        /// <returns>Note tag</returns>
        Task<NoteTag> GetNoteTagByName(string name);

        /// <summary>
        /// Inserts a note tag
        /// </summary>
        /// <param name="noteTag">Note tag</param>
        Task InsertNoteTag(NoteTag noteTag);

        /// <summary>
        /// Update a note tag
        /// </summary>
        /// <param name="noteTag">Note tag</param>
        Task UpdateNoteTag(NoteTag noteTag);

        /// <summary>
        /// Get number of notes
        /// </summary>
        /// <param name="noteTagId">Note tag identifier</param>
        /// <returns>Number of notes</returns>
        Task<int> GetNoteCount(string noteTagId);
    }
}
