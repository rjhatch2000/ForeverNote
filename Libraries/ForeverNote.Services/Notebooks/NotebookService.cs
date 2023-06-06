using ForeverNote.Core;
using ForeverNote.Core.Caching;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Notes;
using ForeverNote.Core.Domain.Notebooks;
using ForeverNote.Services.Events;
using ForeverNote.Services.Localization;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Notebooks
{
    /// <summary>
    /// Notebook service
    /// </summary>
    public partial class NotebookService : INotebookService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : notebook ID
        /// </remarks>
        private const string CATEGORIES_BY_ID_KEY = "ForeverNote.notebook.id-{0}";
        /// <summary>
        /// Key for caching 
        /// </summary>
        /// <remarks>
        /// {0} : parent notebook ID
        /// {1} : current user ID
        /// {2} : include all levels (child)
        /// </remarks>
        private const string CATEGORIES_BY_PARENT_CATEGORY_ID_KEY = "ForeverNote.notebook.byparent-{0}-{1}-{2}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : notebook ID
        /// {1} : page index
        /// {2} : page size
        /// {3} : current user ID
        /// </remarks>
        private const string PRODUCTCATEGORIES_ALLBYCATEGORYID_KEY = "ForeverNote.notenotebook.allbynotebookid-{0}-{1}-{2}-{3}-{4}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CATEGORIES_PATTERN_KEY = "ForeverNote.notebook.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTCATEGORIES_PATTERN_KEY = "ForeverNote.notenotebook.";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>        
        private const string PRODUCTS_PATTERN_KEY = "ForeverNote.note.";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : note ID
        /// </remarks>
        private const string PRODUCTS_BY_ID_KEY = "ForeverNote.note.id-{0}";


        #endregion

        #region Fields

        private readonly IRepository<Notebook> _notebookRepository;
        private readonly IRepository<Note> _noteRepository;
        private readonly IWorkContext _workContext;
        private readonly IMediator _mediator;
        private readonly ICacheManager _cacheManager;
        private readonly CatalogSettings _catalogSettings;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="notebookRepository">Notebook repository</param>
        /// <param name="workContext">Work context</param>
        /// <param name="aclService">ACL service</param>
        /// <param name="catalogSettings">Catalog settings</param>
        public NotebookService(ICacheManager cacheManager,
            IRepository<Notebook> notebookRepository,
            IRepository<Note> noteRepository,
            IWorkContext workContext,
            IMediator mediator,
            CatalogSettings catalogSettings
        )
        {
            _cacheManager = cacheManager;
            _notebookRepository = notebookRepository;
            _noteRepository = noteRepository;
            _workContext = workContext;
            _mediator = mediator;
            _catalogSettings = catalogSettings;
        }

        #endregion

        #region Utilities

        protected IList<Notebook> SortNotebooksForTree(IList<Notebook> source, string parentId = "", bool ignoreNotebooksWithoutExistingParent = false)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var result = new List<Notebook>();

            foreach (var cat in source.Where(c => c.ParentNotebookId == parentId).ToList())
            {
                result.Add(cat);
                result.AddRange(SortNotebooksForTree(source, cat.Id, true));
            }
            if (!ignoreNotebooksWithoutExistingParent && result.Count != source.Count)
            {
                //find notebooks without parent in provided notebook source and insert them into result
                foreach (var cat in source)
                    if (result.FirstOrDefault(x => x.Id == cat.Id) == null)
                        result.Add(cat);
            }
            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete notebook
        /// </summary>
        /// <param name="notebook">Notebook</param>
        public virtual async Task DeleteNotebook(Notebook notebook)
        {
            if (notebook == null)
                throw new ArgumentNullException("notebook");

            //reset a "Parent notebook" property of all child subnotebooks
            var subnotebooks = await GetAllNotebooksByParentNotebookId(notebook.Id, true);
            foreach (var subnotebook in subnotebooks)
            {
                subnotebook.ParentNotebookId = "";
                await UpdateNotebook(subnotebook);
            }

            var builder = Builders<Note>.Update;
            var updatefilter = builder.PullFilter(x => x.NoteNotebooks, y => y.NotebookId == notebook.Id);
            await _noteRepository.Collection.UpdateManyAsync(new BsonDocument(), updatefilter);

            await _notebookRepository.DeleteAsync(notebook);

            await _cacheManager.RemoveByPrefix(PRODUCTS_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(CATEGORIES_PATTERN_KEY);

            //event notification
            await _mediator.EntityDeleted(notebook);
        }

        /// <summary>
        /// Gets all notebooks
        /// </summary>
        /// <param name="notebookName">Notebook name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Notebooks</returns>
        public virtual async Task<IPagedList<Notebook>> GetAllNotebooks(string notebookName = "",
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from c in _notebookRepository.Table
                        select c;

            if (!string.IsNullOrWhiteSpace(notebookName))
                query = query.Where(m => m.Name != null && m.Name.ToLower().Contains(notebookName.ToLower()));

            query = query.OrderBy(c => c.ParentNotebookId).ThenBy(c => c.DisplayOrder).ThenBy(c => c.Name);
            var unsortedNotebooks = query.ToList();
            //sort notebooks
            var sortedNotebooks = SortNotebooksForTree(unsortedNotebooks);

            //paging
            return await Task.FromResult(new PagedList<Notebook>(sortedNotebooks, pageIndex, pageSize));
        }

        /// <summary>
        /// Gets all notebooks filtered by parent notebook identifier
        /// </summary>
        /// <param name="parentNotebookId">Parent notebook identifier</param>
        /// <param name="includeAllLevels">A value indicating whether we should load all child levels</param>
        /// <returns>Notebooks</returns>
        public virtual async Task<IList<Notebook>> GetAllNotebooksByParentNotebookId(string parentNotebookId = "",
            bool includeAllLevels = false)
        {
            var user = _workContext.CurrentUser;
            string key = string.Format(CATEGORIES_BY_PARENT_CATEGORY_ID_KEY, parentNotebookId, user.Id, includeAllLevels);
            return await _cacheManager.GetAsync(key, async () =>
            {
                var builder = Builders<Notebook>.Filter;
                var filter = builder.Where(c => c.ParentNotebookId == parentNotebookId);

                var notebooks = _notebookRepository.Collection.Find(filter).SortBy(x => x.DisplayOrder).ToList();
                if (includeAllLevels)
                {
                    var childNotebooks = new List<Notebook>();
                    //add child levels
                    foreach (var notebook in notebooks)
                    {
                        childNotebooks.AddRange(await GetAllNotebooksByParentNotebookId(notebook.Id, includeAllLevels));
                    }
                    notebooks.AddRange(childNotebooks);
                }
                return notebooks;
            });
        }

        /// <summary>
        /// Gets all notebooks displayed on the home page
        /// </summary>
        /// <returns>Notebooks</returns>
        public virtual async Task<IList<Notebook>> GetAllNotebooksDisplayedOnHomePage()
        {
            var builder = Builders<Notebook>.Filter;
            var filter = builder.Eq(x => x.ShowOnHomePage, true);
            var query = _notebookRepository.Collection.Find(filter).SortBy(x => x.DisplayOrder);

            var notebooks = await query.ToListAsync();

            return notebooks;
        }

        /// <summary>
        /// Gets all notebooks displayed on the home page
        /// </summary>
        /// <returns>Notebooks</returns>
        public virtual async Task<IList<Notebook>> GetAllNotebooksFeaturedNotesOnHomePage()
        {
            var builder = Builders<Notebook>.Filter;
            var filter = builder.Eq(x => x.FeaturedNotesOnHomePage, true);
            var query = _notebookRepository.Collection.Find(filter).SortBy(x => x.DisplayOrder);

            var notebooks = await query.ToListAsync();

            return notebooks;
        }

        /// <summary>
        /// Gets all notebooks displayed on the home page
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Notebooks</returns>
        public virtual async Task<IList<Notebook>> GetAllNotebooksSearchBox()
        {
            var builder = Builders<Notebook>.Filter;
            var filter = builder.Eq(x => x.ShowOnSearchBox, true);
            var query = _notebookRepository.Collection.Find(filter).SortBy(x => x.SearchBoxDisplayOrder);
            var notebooks = await query.ToListAsync();

            return notebooks;
        }

        /// <summary>
        /// Get notebook breadcrumb 
        /// </summary>
        /// <param name="notebook">Notebook</param>
        /// <returns>Notebook breadcrumb </returns>
        public virtual async Task<IList<Notebook>> GetNotebookBreadCrumb(Notebook notebook)
        {
            var result = new List<Notebook>();

            //used to prevent circular references
            var alreadyProcessedNotebookIds = new List<string>();

            while (notebook != null && //not null                
                !alreadyProcessedNotebookIds.Contains(notebook.Id)) //prevent circular references
            {
                result.Add(notebook);

                alreadyProcessedNotebookIds.Add(notebook.Id);

                notebook = await GetNotebookById(notebook.ParentNotebookId);
            }
            result.Reverse();
            return result;
        }

        /// <summary>
        /// Get notebook breadcrumb 
        /// </summary>
        /// <param name="notebook">Notebook</param>
        /// <param name="allNotebooks">All notebooks</param>
        /// <returns>Notebook breadcrumb </returns>
        public virtual IList<Notebook> GetNotebookBreadCrumb(Notebook notebook, IList<Notebook> allNotebooks)
        {
            var result = new List<Notebook>();

            //used to prevent circular references
            var alreadyProcessedNotebookIds = new List<string>();

            while (notebook != null && //not null                
                !alreadyProcessedNotebookIds.Contains(notebook.Id)) //prevent circular references
            {
                result.Add(notebook);

                alreadyProcessedNotebookIds.Add(notebook.Id);

                notebook = (from c in allNotebooks
                            where c.Id == notebook.ParentNotebookId
                            select c).FirstOrDefault();
            }
            result.Reverse();
            return result;
        }

        /// <summary>
        /// Get formatted notebook breadcrumb 
        /// Note: ACL mapping is ignored
        /// </summary>
        /// <param name="notebook">Notebook</param>
        /// <param name="separator">Separator</param>
        /// <param name="languageId">Language identifier for localization</param>
        /// <returns>Formatted breadcrumb</returns>
        public virtual async Task<string> GetFormattedBreadCrumb(Notebook notebook, string separator = ">>", string languageId = "")
        {
            string result = string.Empty;

            var breadcrumb = await GetNotebookBreadCrumb(notebook);
            for (int i = 0; i <= breadcrumb.Count - 1; i++)
            {
                var notebookName = breadcrumb[i].GetLocalized(x => x.Name, languageId);
                result = string.IsNullOrEmpty(result)
                    ? notebookName
                    : string.Format("{0} {1} {2}", result, separator, notebookName);
            }

            return result;
        }
        /// <summary>
        /// Get formatted notebook breadcrumb 
        /// Note: ACL mapping is ignored
        /// </summary>
        /// <param name="notebook">Notebook</param>
        /// <param name="allNotebooks">All notebooks</param>
        /// <param name="separator">Separator</param>
        /// <param name="languageId">Language identifier for localization</param>
        /// <returns>Formatted breadcrumb</returns>
        public virtual string GetFormattedBreadCrumb(Notebook notebook,
            IList<Notebook> allNotebooks, string separator = ">>", string languageId = "")
        {
            string result = string.Empty;

            var breadcrumb = GetNotebookBreadCrumb(notebook, allNotebooks);
            for (int i = 0; i <= breadcrumb.Count - 1; i++)
            {
                var notebookName = breadcrumb[i].GetLocalized(x => x.Name, languageId);
                result = string.IsNullOrEmpty(result)
                    ? notebookName
                    : string.Format("{0} {1} {2}", result, separator, notebookName);
            }

            return result;
        }

        /// <summary>
        /// Gets a notebook
        /// </summary>
        /// <param name="notebookId">Notebook identifier</param>
        /// <returns>Notebook</returns>
        public virtual async Task<Notebook> GetNotebookById(string notebookId)
        {
            string key = string.Format(CATEGORIES_BY_ID_KEY, notebookId);
            return await _cacheManager.GetAsync(key, () => _notebookRepository.GetByIdAsync(notebookId));
        }

        /// <summary>
        /// Inserts notebook
        /// </summary>
        /// <param name="notebook">Notebook</param>
        public virtual async Task InsertNotebook(Notebook notebook)
        {
            if (notebook == null)
                throw new ArgumentNullException("notebook");

            await _notebookRepository.InsertAsync(notebook);

            //cache
            await _cacheManager.RemoveByPrefix(CATEGORIES_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(PRODUCTCATEGORIES_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(PRODUCTS_PATTERN_KEY);

            //event notification
            await _mediator.EntityInserted(notebook);
        }

        /// <summary>
        /// Updates the notebook
        /// </summary>
        /// <param name="notebook">Notebook</param>
        public virtual async Task UpdateNotebook(Notebook notebook)
        {
            if (notebook == null)
                throw new ArgumentNullException("notebook");
            if (string.IsNullOrEmpty(notebook.ParentNotebookId))
                notebook.ParentNotebookId = "";

            //validate notebook hierarchy
            var parentNotebook = await GetNotebookById(notebook.ParentNotebookId);
            while (parentNotebook != null)
            {
                if (notebook.Id == parentNotebook.Id)
                {
                    notebook.ParentNotebookId = "";
                    break;
                }
                parentNotebook = await GetNotebookById(parentNotebook.ParentNotebookId);
            }

            await _notebookRepository.UpdateAsync(notebook);

            //cache
            await _cacheManager.RemoveByPrefix(CATEGORIES_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(PRODUCTCATEGORIES_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(PRODUCTS_PATTERN_KEY);

            //event notification
            await _mediator.EntityUpdated(notebook);
        }

        /// <summary>
        /// Deletes a note notebook mapping
        /// </summary>
        /// <param name="noteNotebook">Note notebook</param>
        public virtual async Task DeleteNoteNotebook(NoteNotebook noteNotebook)
        {
            if (noteNotebook == null)
                throw new ArgumentNullException("noteNotebook");

            var updatebuilder = Builders<Note>.Update;
            var update = updatebuilder.Pull(p => p.NoteNotebooks, noteNotebook);
            await _noteRepository.Collection.UpdateOneAsync(new BsonDocument("_id", noteNotebook.NoteId), update);

            //cache
            await _cacheManager.RemoveByPrefix(CATEGORIES_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(PRODUCTCATEGORIES_PATTERN_KEY);
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, noteNotebook.NoteId));

            //event notification
            await _mediator.EntityDeleted(noteNotebook);

        }

        /// <summary>
        /// Gets note notebook mapping collection
        /// </summary>
        /// <param name="notebookId">Notebook identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Note a notebook mapping collection</returns>
        public virtual async Task<IPagedList<NoteNotebook>> GetNoteNotebooksByNotebookId(string notebookId,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            if (string.IsNullOrEmpty(notebookId))
                return new PagedList<NoteNotebook>(new List<NoteNotebook>(), pageIndex, pageSize);

            string key = string.Format(PRODUCTCATEGORIES_ALLBYCATEGORYID_KEY, notebookId, pageIndex, pageSize, _workContext.CurrentUser.Id);
            return await _cacheManager.GetAsync(key, () =>
            {
                var query = _noteRepository.Table.Where(x => x.NoteNotebooks.Any(y => y.NotebookId == notebookId));

                var query_noteNotebooks = from prod in query
                                              from pc in prod.NoteNotebooks
                                              select new SerializeNoteNotebook
                                              {
                                                  NotebookId = pc.NotebookId,
                                                  DisplayOrder = pc.DisplayOrder,
                                                  Id = pc.Id,
                                                  NoteId = prod.Id,
                                                  IsFeaturedNote = pc.IsFeaturedNote,
                                              };

                query_noteNotebooks = from pm in query_noteNotebooks
                                          where pm.NotebookId == notebookId
                                          orderby pm.DisplayOrder
                                          select pm;

                return Task.FromResult(new PagedList<NoteNotebook>(query_noteNotebooks, pageIndex, pageSize));
            });
        }


        /// <summary>
        /// Inserts a note notebook mapping
        /// </summary>
        /// <param name="noteNotebook">>Note notebook mapping</param>
        public virtual async Task InsertNoteNotebook(NoteNotebook noteNotebook)
        {
            if (noteNotebook == null)
                throw new ArgumentNullException("noteNotebook");

            var updatebuilder = Builders<Note>.Update;
            var update = updatebuilder.AddToSet(p => p.NoteNotebooks, noteNotebook);
            await _noteRepository.Collection.UpdateOneAsync(new BsonDocument("_id", noteNotebook.NoteId), update);

            //cache
            await _cacheManager.RemoveByPrefix(CATEGORIES_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(PRODUCTCATEGORIES_PATTERN_KEY);
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, noteNotebook.NoteId));

            //event notification
            await _mediator.EntityInserted(noteNotebook);
        }

        /// <summary>
        /// Updates the note notebook mapping 
        /// </summary>
        /// <param name="noteNotebook">>Note notebook mapping</param>
        public virtual async Task UpdateNoteNotebook(NoteNotebook noteNotebook)
        {
            if (noteNotebook == null)
                throw new ArgumentNullException("noteNotebook");

            var builder = Builders<Note>.Filter;
            var filter = builder.Eq(x => x.Id, noteNotebook.NoteId);
            filter = filter & builder.Where(x => x.NoteNotebooks.Any(y => y.Id == noteNotebook.Id));
            var update = Builders<Note>.Update
                .Set(x => x.NoteNotebooks.ElementAt(-1).NotebookId, noteNotebook.NotebookId)
                .Set(x => x.NoteNotebooks.ElementAt(-1).IsFeaturedNote, noteNotebook.IsFeaturedNote)
                .Set(x => x.NoteNotebooks.ElementAt(-1).DisplayOrder, noteNotebook.DisplayOrder);

            await _noteRepository.Collection.UpdateManyAsync(filter, update);

            //cache
            await _cacheManager.RemoveByPrefix(CATEGORIES_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(PRODUCTCATEGORIES_PATTERN_KEY);
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, noteNotebook.NoteId));

            //event notification
            await _mediator.EntityUpdated(noteNotebook);
        }
        #endregion

        public class SerializeNoteNotebook : NoteNotebook
        {

        }

    }
}
