using ForeverNote.Core;
using ForeverNote.Core.Domain.Notes;
using ForeverNote.Core.Domain.Notebooks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Notebooks
{
    /// <summary>
    /// Notebook service interface
    /// </summary>
    public partial interface INotebookService
    {
        /// <summary>
        /// Delete notebook
        /// </summary>
        /// <param name="notebook">Notebook</param>
        Task DeleteNotebook(Notebook notebook);

        /// <summary>
        /// Gets all notebooks
        /// </summary>
        /// <param name="notebookName">Notebook name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Notebooks</returns>
        Task<IPagedList<Notebook>> GetAllNotebooks(string notebookName = "",
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets all notebooks filtered by parent notebook identifier
        /// </summary>
        /// <param name="parentNotebookId">Parent notebook identifier</param>
        /// <param name="includeAllLevels">A value indicating whether we should load all child levels</param>
        /// <returns>Notebooks</returns>
        Task<IList<Notebook>> GetAllNotebooksByParentNotebookId(
            string parentNotebookId = "",
            bool includeAllLevels = false
        );

        /// <summary>
        /// Gets all notebooks displayed on the home page
        /// </summary>
        /// <returns>Notebooks</returns>
        Task<IList<Notebook>> GetAllNotebooksDisplayedOnHomePage();

        /// <summary>
        /// Gets all notebooks displayed on the home page - featured notes
        /// </summary>
        Task<IList<Notebook>> GetAllNotebooksFeaturedNotesOnHomePage();

        /// <summary>
        /// Gets all notebooks displayed search box
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Notebooks</returns>
        Task<IList<Notebook>> GetAllNotebooksSearchBox();

        /// <summary>
        /// Get notebook breadcrumb 
        /// </summary>
        /// <param name="notebook">Notebook</param>
        /// <returns>Notebook breadcrumb </returns>
        Task<IList<Notebook>> GetNotebookBreadCrumb(Notebook notebook);

        /// <summary>
        /// Get notebook breadcrumb 
        /// </summary>
        /// <param name="notebook">Notebook</param>
        /// <param name="allNotebooks">All notebooks</param>
        /// <returns>Notebook breadcrumb </returns>
        IList<Notebook> GetNotebookBreadCrumb(Notebook notebook, IList<Notebook> allNotebooks);

        /// <summary>
        /// Get formatted notebook breadcrumb 
        /// Note: ACL mapping is ignored
        /// </summary>
        /// <param name="notebook">Notebook</param>
        /// <param name="separator">Separator</param>
        /// <param name="languageId">Language identifier for localization</param>
        /// <returns>Formatted breadcrumb</returns>
        Task<string> GetFormattedBreadCrumb(Notebook notebook, string separator = ">>", string languageId = "");

        /// <summary>
        /// Get formatted notebook breadcrumb 
        /// Note: ACL mapping is ignored
        /// </summary>
        /// <param name="notebook">Notebook</param>
        /// <param name="allNotebooks">All notebooks</param>
        /// <param name="separator">Separator</param>
        /// <param name="languageId">Language identifier for localization</param>
        /// <returns>Formatted breadcrumb</returns>
        string GetFormattedBreadCrumb(Notebook notebook,
            IList<Notebook> allNotebooks, string separator = ">>", string languageId = "");

        /// <summary>
        /// Gets a notebook
        /// </summary>
        /// <param name="notebookId">Notebook identifier</param>
        /// <returns>Notebook</returns>
        Task<Notebook> GetNotebookById(string notebookId);

        /// <summary>
        /// Inserts notebook
        /// </summary>
        /// <param name="notebook">Notebook</param>
        Task InsertNotebook(Notebook notebook);

        /// <summary>
        /// Updates the notebook
        /// </summary>
        /// <param name="notebook">Notebook</param>
        Task UpdateNotebook(Notebook notebook);

        /// <summary>
        /// Deletes a note notebook mapping
        /// </summary>
        /// <param name="noteNotebook">Note notebook</param>
        Task DeleteNoteNotebook(NoteNotebook noteNotebook);

        /// <summary>
        /// Gets note notebook mapping collection
        /// </summary>
        /// <param name="notebookId">Notebook identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Note a notebook mapping collection</returns>
        Task<IPagedList<NoteNotebook>> GetNoteNotebooksByNotebookId(string notebookId,
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Inserts a note notebook mapping
        /// </summary>
        /// <param name="noteNotebook">>Note notebook mapping</param>
        Task InsertNoteNotebook(NoteNotebook noteNotebook);

        /// <summary>
        /// Updates the note notebook mapping 
        /// </summary>
        /// <param name="noteNotebook">>Note notebook mapping</param>
        Task UpdateNoteNotebook(NoteNotebook noteNotebook);


    }
}
