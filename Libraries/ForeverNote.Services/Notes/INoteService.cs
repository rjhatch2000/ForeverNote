using ForeverNote.Core;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Notes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Notes
{
    /// <summary>
    /// Note service
    /// </summary>
    public partial interface INoteService
    {
        #region Notes

        /// <summary>
        /// Delete a note
        /// </summary>
        /// <param name="note">Note</param>
        Task DeleteNote(Note note);

        /// <summary>
        /// Gets all notes displayed on the home page
        /// </summary>
        /// <returns>Notes</returns>
        Task<IList<Note>> GetAllNotesDisplayedOnHomePage();

        /// <summary>
        /// Gets recommended notes for user roles
        /// </summary>
        /// <returns>Notes</returns>
        Task<IList<Note>> GetSuggestedNotes(string[] userTagIds);

        /// <summary>
        /// Gets note
        /// </summary>
        /// <param name="noteId">Note identifier</param>
        /// <param name="fromDB">get data from db (not from cache)</param>
        /// <returns>Note</returns>
        Task<Note> GetNoteById(string noteId, bool fromDB = false);

        /// <summary>
        /// Gets note from note or note deleted
        /// </summary>
        /// <param name="noteId">Note identifier</param>
        /// <returns>Note</returns>
        Task<Note> GetNoteByIdIncludeArch(string noteId);

        /// <summary>
        /// Gets notes by identifier
        /// </summary>
        /// <param name="noteIds">Note identifiers</param>
        /// <returns>Notes</returns>
        Task<IList<Note>> GetNotesByIds(string[] noteIds);

        /// <summary>
        /// Inserts a note
        /// </summary>
        /// <param name="note">Note</param>
        Task InsertNote(Note note);

        /// <summary>
        /// Updates the note
        /// </summary>
        /// <param name="note">Note</param>
        Task UpdateNote(Note note);

        /// <summary>
        /// Updates most view on the note
        /// </summary>
        /// <param name="noteId">NoteId</param>
        /// <param name="qty">Count</param>
        Task UpdateMostView(string noteId, int qty);

        /// <summary>
        /// Get (visible) note number in certain notebook
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="notebookIds">Notebook identifiers</param>
        /// <returns>Note number</returns>
        int GetNotebookNoteNumber(User user, IList<string> notebookIds = null);

        /// <summary>
        /// Search notes
        /// </summary>
        /// <param name="filterableSpecificationAttributeOptionIds">The specification attribute option identifiers applied to loaded notes (all pages)</param>
        /// <param name="loadFilterableSpecificationAttributeOptionIds">A value indicating whether we should load the specification attribute option identifiers applied to loaded notes (all pages)</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="notebookIds">Notebook identifiers</param>
        /// <param name="manufacturerId">Manufacturer identifier; "" to load all records</param>
        /// <param name="vendorId">Vendor identifier; "" to load all records</param>
        /// <param name="warehouseId">Warehouse identifier; "" to load all records</param>
        /// <param name="visibleIndividuallyOnly">A values indicating whether to load only notes marked as "visible individually"; "false" to load all records; "true" to load "visible individually" only</param>
        /// <param name="featuredNotes">A value indicating whether loaded notes are marked as featured (relates only to notebooks and manufacturers). 0 to load featured notes only, 1 to load not featured notes only, null to load all notes</param>
        /// <param name="priceMin">Minimum price; null to load all records</param>
        /// <param name="priceMax">Maximum price; null to load all records</param>
        /// <param name="noteTag">Note tag name; "" to load all records</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search by a specified "keyword" in note descriptions</param>
        /// <param name="searchSku">A value indicating whether to search by a specified "keyword" in note SKU</param>
        /// <param name="searchNoteTags">A value indicating whether to search by a specified "keyword" in note tags</param>
        /// <param name="languageId">Language identifier (search for text searching)</param>
        /// <param name="filteredSpecs">Filtered note specification identifiers</param>
        /// <param name="orderBy">Order by</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="overridePublished">
        /// null - process "Published" property according to "showHidden" parameter
        /// true - load only "Published" notes
        /// false - load only "Unpublished" notes
        /// </param>
        /// <returns>Notes</returns>
        Task<(IPagedList<Note> notes, IList<string> filterableSpecificationAttributeOptionIds)> SearchNotes(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            IList<string> notebookIds = null,
            bool markedAsNewOnly = false,
            bool? featuredNotes = null,
            string noteTag = "",
            string keywords = null,
            bool searchDescriptions = false,
            bool searchNoteTags = false,
            string languageId = "",
            NoteSortingEnum orderBy = NoteSortingEnum.Position
        );

        /// <summary>
        /// Update Interval properties
        /// </summary>
        /// <param name="noteId">Note Id</param>
        /// <param name="Interval">Interval</param>
        /// <param name="IntervalUnit">Interval unit</param>
        /// <param name="includeBothDates">Include both dates</param>
        Task UpdateIntervalProperties(string noteId, int interval, IntervalUnit intervalUnit, bool includeBothDates);


        #endregion

        #region Note Tags

        /// <summary>
        /// Inserts a note tags
        /// </summary>
        /// <param name="noteTag">Note Tag</param>
        Task InsertNoteTag(NoteTag noteTag);

        /// <summary>
        /// Delete a note tags
        /// </summary>
        /// <param name="noteTag">Note Tag</param>
        Task DeleteNoteTag(NoteTag noteTag);

        #endregion

        #region Note pictures

        /// <summary>
        /// Deletes a note picture
        /// </summary>
        /// <param name="notePicture">Note picture</param>
        Task DeleteNotePicture(NotePicture notePicture);

        /// <summary>
        /// Inserts a note picture
        /// </summary>
        /// <param name="notePicture">Note picture</param>
        Task InsertNotePicture(NotePicture notePicture);

        /// <summary>
        /// Updates a note picture
        /// </summary>
        /// <param name="notePicture">Note picture</param>
        Task UpdateNotePicture(NotePicture notePicture);

        #endregion
    }
}
