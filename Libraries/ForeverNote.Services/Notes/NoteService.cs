using ForeverNote.Core;
using ForeverNote.Core.Caching;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Core.Domain.Notes;
using ForeverNote.Services.Events;
using ForeverNote.Services.Queries.Models.Catalog;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Notes
{
    /// <summary>
    /// Note service
    /// </summary>
    public partial class NoteService : INoteService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : note ID
        /// </remarks>
        private const string PRODUCTS_BY_ID_KEY = "ForeverNote.note.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTS_PATTERN_KEY = "ForeverNote.note.";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTS_SHOWONHOMEPAGE = "ForeverNote.note.showonhomepage";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user ID
        /// </remarks>
        private const string PRODUCTS_CUSTOMER_ROLE = "ForeverNote.note.cr-{0}";
        private const string PRODUCTS_CUSTOMER_ROLE_PATTERN = "ForeverNote.note.cr";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user ID
        /// </remarks>
        private const string PRODUCTS_CUSTOMER_TAG = "ForeverNote.note.ct-{0}";
        private const string PRODUCTS_CUSTOMER_TAG_PATTERN = "ForeverNote.note.ct";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : user ID
        /// </remarks>
        private const string PRODUCTS_CUSTOMER_PERSONAL = "ForeverNote.note.personal-{0}";
        private const string PRODUCTS_CUSTOMER_PERSONAL_PATTERN = "ForeverNote.note.personal";

        #endregion

        #region Fields

        private readonly IRepository<Note> _noteRepository;
        private readonly IRepository<NoteTag> _noteTagRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserTagNote> _userTagNoteRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly IMediator _mediator;
        private readonly CatalogSettings _catalogSettings;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public NoteService(
            ICacheManager cacheManager,
            IRepository<Note> noteRepository,
            IRepository<User> userRepository,
            IRepository<UserTagNote> userTagNoteRepository,
            IRepository<NoteTag> noteTagRepository,
            IWorkContext workContext,
            IMediator mediator,
            CatalogSettings catalogSettings
        )
        {
            _cacheManager = cacheManager;
            _noteRepository = noteRepository;
            _userRepository = userRepository;
            _userTagNoteRepository = userTagNoteRepository;
            _noteTagRepository = noteTagRepository;
            _workContext = workContext;
            _mediator = mediator;
            _catalogSettings = catalogSettings;
        }

        #endregion

        #region Methods

        #region Notes

        /// <summary>
        /// Delete a note
        /// </summary>
        /// <param name="note">Note</param>
        public virtual async Task DeleteNote(Note note)
        {
            if (note == null)
                throw new ArgumentNullException("note");

            //delete note tags
            var existingNoteTags = _noteTagRepository.Table.Where(x => note.NoteTags.ToList().Contains(x.Name)).ToList();
            foreach (var tag in existingNoteTags)
            {
                tag.NoteId = note.Id;
                await DeleteNoteTag(tag);
            }

            //deleted note
            await _noteRepository.DeleteAsync(note);

            //cache
            await _cacheManager.RemoveByPrefix(PRODUCTS_PATTERN_KEY);

        }

        /// <summary>
        /// Gets all notes displayed on the home page
        /// </summary>
        /// <returns>Notes</returns>
        public virtual async Task<IList<Note>> GetAllNotesDisplayedOnHomePage()
        {
            var builder = Builders<Note>.Filter;
            var filter = builder.Eq(x => x.ShowOnHomePage, true);
            var query = _noteRepository.Collection.Find(filter).SortBy(x => x.DisplayOrder).ThenBy(x => x.Name);

            var notes = await query.ToListAsync();

            //availability dates
            notes = notes.ToList();
            return notes;
        }

        /// <summary>
        /// Gets note
        /// </summary>
        /// <param name="noteId">Note identifier</param>
        /// <param name="fromDB">get data from db (not from cache)</param>
        /// <returns>Note</returns>
        public virtual async Task<Note> GetNoteById(string noteId, bool fromDB = false)
        {
            if (string.IsNullOrEmpty(noteId))
                return null;

            if (fromDB)
                return await _noteRepository.GetByIdAsync(noteId);

            var key = string.Format(PRODUCTS_BY_ID_KEY, noteId);
            return await _cacheManager.GetAsync(key, () => _noteRepository.GetByIdAsync(noteId));
        }

        /// <summary>
        /// Gets note for order
        /// </summary>
        /// <param name="noteId">Note identifier</param>
        /// <returns>Note</returns>
        public virtual async Task<Note> GetNoteByIdIncludeArch(string noteId)
        {
            if (string.IsNullOrEmpty(noteId))
                return null;
            var note = await _noteRepository.GetByIdAsync(noteId);
            return note;
        }


        /// <summary>
        /// Get notes by identifiers
        /// </summary>
        /// <param name="noteIds">Note identifiers</param>
        /// <returns>Notes</returns>
        public virtual async Task<IList<Note>> GetNotesByIds(string[] noteIds)
        {
            if (noteIds == null || noteIds.Length == 0)
                return new List<Note>();

            var builder = Builders<Note>.Filter;
            var filter = builder.Where(x => noteIds.Contains(x.Id));
            var notes = await _noteRepository.Collection.Find(filter).ToListAsync();

            //sort by passed identifiers
            var sortedNotes = new List<Note>();
            foreach (string id in noteIds)
            {
                var note = notes.Find(x => x.Id == id);
                if (note != null)
                    sortedNotes.Add(note);
            }

            return sortedNotes;
        }

        /// <summary>
        /// Inserts a note
        /// </summary>
        /// <param name="note">Note</param>
        public virtual async Task InsertNote(Note note)
        {
            if (note == null)
                throw new ArgumentNullException("note");

            //insert
            await _noteRepository.InsertAsync(note);

            //clear cache
            await _cacheManager.RemoveByPrefix(PRODUCTS_PATTERN_KEY);

            //event notification
            await _mediator.EntityInserted(note);
        }

        /// <summary>
        /// Updates the note
        /// </summary>
        /// <param name="note">Note</param>
        public virtual async Task UpdateNote(Note note)
        {
            if (note == null)
                throw new ArgumentNullException("note");
            var oldNote = await _noteRepository.GetByIdAsync(note.Id);
            //update
            var builder = Builders<Note>.Filter;
            var filter = builder.Eq(x => x.Id, note.Id);
            var update = Builders<Note>.Update
                .Set(x => x.AdminComment, note.AdminComment)
                .Set(x => x.AllowBackInStockSubscriptions, note.AllowBackInStockSubscriptions)
                .Set(x => x.CreatedOnUtc, note.CreatedOnUtc)
                .Set(x => x.UserRoles, note.UserRoles)
                .Set(x => x.DeliveryDateId, note.DeliveryDateId)
                .Set(x => x.DisplayOrder, note.DisplayOrder)
                .Set(x => x.DisplayOrderNotebook, note.DisplayOrderNotebook)
                .Set(x => x.Flag, note.Flag)
                .Set(x => x.FullDescription, note.FullDescription)
                .Set(x => x.IncBothDate, note.IncBothDate)
                .Set(x => x.IsRecurring, note.IsRecurring)
                .Set(x => x.Locales, note.Locales)
                .Set(x => x.MarkAsNew, note.MarkAsNew)
                .Set(x => x.MarkAsNewStartDateTimeUtc, note.MarkAsNewStartDateTimeUtc)
                .Set(x => x.MarkAsNewEndDateTimeUtc, note.MarkAsNewEndDateTimeUtc)
                .Set(x => x.Name, note.Name)
                .Set(x => x.OnSale, note.OnSale)
                .Set(x => x.RecurringCycleLength, note.RecurringCycleLength)
                .Set(x => x.RecurringCyclePeriodId, note.RecurringCyclePeriodId)
                .Set(x => x.RecurringTotalCycles, note.RecurringTotalCycles)
                .Set(x => x.ShortDescription, note.ShortDescription)
                .Set(x => x.ShowOnHomePage, note.ShowOnHomePage)
                .Set(x => x.SubjectToAcl, note.SubjectToAcl)
                .Set(x => x.UnitId, note.UnitId)
                .Set(x => x.GenericAttributes, note.GenericAttributes)
                .CurrentDate("UpdatedOnUtc");

            await _noteRepository.Collection.UpdateOneAsync(filter, update);

            //cache
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, note.Id));
            await _cacheManager.RemoveByPrefix(PRODUCTS_CUSTOMER_PERSONAL_PATTERN);
            await _cacheManager.RemoveByPrefix(PRODUCTS_CUSTOMER_ROLE_PATTERN);
            await _cacheManager.RemoveByPrefix(PRODUCTS_CUSTOMER_TAG_PATTERN);

            //event notification
            await _mediator.EntityUpdated(note);
        }

        public virtual async Task UpdateMostView(string noteId, int qty)
        {
            var update = new UpdateDefinitionBuilder<Note>().Inc(x => x.Viewed, qty);
            await _noteRepository.Collection.UpdateManyAsync(x => x.Id == noteId, update);
        }

        /// <summary>
        /// Get (visible) note number in certain notebook
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="notebookIds">Notebook identifiers</param>
        /// <returns>Note number</returns>
        public virtual int GetNotebookNoteNumber(User user, IList<string> notebookIds = null)
        {
            //validate "notebookIds" parameter
            if (notebookIds != null && notebookIds.Contains(""))
                notebookIds.Remove("");

            var builder = Builders<Note>.Filter;
            var filter = builder.Where(p => true);
            ////notebook filtering
            if (notebookIds != null && notebookIds.Any())
            {
                filter = filter & builder.Where(p => p.NoteNotebooks.Any(x => notebookIds.Contains(x.NotebookId)));
            }

            return Convert.ToInt32(_noteRepository.Collection.Find(filter).CountDocuments());
        }

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
        public virtual async Task<(IPagedList<Note> notes, IList<string> filterableSpecificationAttributeOptionIds)> SearchNotes(
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
        )
        {

            var model = await _mediator.Send(new GetSearchNotesQuery()
            {
                User = _workContext.CurrentUser,
                PageIndex = pageIndex,
                PageSize = pageSize,
                NotebookIds = notebookIds,
                MarkedAsNewOnly = markedAsNewOnly,
                FeaturedNotes = featuredNotes,
                NoteTag = noteTag,
                Keywords = keywords,
                SearchDescriptions = searchDescriptions,
                SearchNoteTags = searchNoteTags,
                LanguageId = languageId,
                OrderBy = orderBy,
            });

            return model;
        }

        /// <summary>
        /// Update Interval properties
        /// </summary>
        /// <param name="Interval">Interval</param>
        /// <param name="IntervalUnit">Interval unit</param>
        /// <param name="includeBothDates">Include both dates</param>
        public virtual async Task UpdateIntervalProperties(string noteId, int interval, IntervalUnit intervalUnit, bool includeBothDates)
        {
            var note = await GetNoteById(noteId);
            if (note == null)
                throw new ArgumentNullException("note");

            var filter = Builders<Note>.Filter.Eq("Id", note.Id);
            var update = Builders<Note>.Update
                    .Set(x => x.Interval, interval)
                    .Set(x => x.IntervalUnitId, (int)intervalUnit)
                    .Set(x => x.IncBothDate, includeBothDates);

            await _noteRepository.Collection.UpdateOneAsync(filter, update);

            //event notification
            await _mediator.EntityUpdated(note);

            //cache
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, note.Id));

        }

        #endregion

        #region Note pictures

        /// <summary>
        /// Deletes a note picture
        /// </summary>
        /// <param name="notePicture">Note picture</param>
        public virtual async Task DeleteNotePicture(NotePicture notePicture)
        {
            if (notePicture == null)
                throw new ArgumentNullException("notePicture");

            var updatebuilder = Builders<Note>.Update;
            var update = updatebuilder.Pull(p => p.NotePictures, notePicture);
            await _noteRepository.Collection.UpdateOneAsync(new BsonDocument("_id", notePicture.NoteId), update);

            //cache
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, notePicture.NoteId));

            //event notification
            await _mediator.EntityDeleted(notePicture);
        }

        /// <summary>
        /// Inserts a note picture
        /// </summary>
        /// <param name="notePicture">Note picture</param>
        public virtual async Task InsertNotePicture(NotePicture notePicture)
        {
            if (notePicture == null)
                throw new ArgumentNullException("notePicture");

            var updatebuilder = Builders<Note>.Update;
            var update = updatebuilder.AddToSet(p => p.NotePictures, notePicture);
            await _noteRepository.Collection.UpdateOneAsync(new BsonDocument("_id", notePicture.NoteId), update);

            //cache
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, notePicture.NoteId));

            //event notification
            await _mediator.EntityInserted(notePicture);
        }

        /// <summary>
        /// Inserts a note tag
        /// </summary>
        /// <param name="notePicture">Note picture</param>
        public virtual async Task InsertNoteTag(NoteTag noteTag)
        {
            if (noteTag == null)
                throw new ArgumentNullException("noteTag");

            var updatebuilder = Builders<Note>.Update;
            var update = updatebuilder.AddToSet(p => p.NoteTags, noteTag.Name);
            await _noteRepository.Collection.UpdateOneAsync(new BsonDocument("_id", noteTag.NoteId), update);

            var builder = Builders<NoteTag>.Filter;
            var filter = builder.Eq(x => x.Id, noteTag.Id);
            var updateTag = Builders<NoteTag>.Update
                .Inc(x => x.Count, 1);
            await _noteTagRepository.Collection.UpdateManyAsync(filter, updateTag);

            //cache
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, noteTag.NoteId));

            //event notification
            await _mediator.EntityInserted(noteTag);
        }

        public virtual async Task DeleteNoteTag(NoteTag noteTag)
        {
            if (noteTag == null)
                throw new ArgumentNullException("noteTag");

            var updatebuilder = Builders<Note>.Update;
            var update = updatebuilder.Pull(p => p.NoteTags, noteTag.Name);
            await _noteRepository.Collection.UpdateOneAsync(new BsonDocument("_id", noteTag.NoteId), update);

            var builder = Builders<NoteTag>.Filter;
            var filter = builder.Eq(x => x.Id, noteTag.Id);
            var updateTag = Builders<NoteTag>.Update
                .Inc(x => x.Count, -1);
            await _noteTagRepository.Collection.UpdateManyAsync(filter, updateTag);

            //cache
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, noteTag.NoteId));

            //event notification
            await _mediator.EntityDeleted(noteTag);
        }

        /// <summary>
        /// Updates a note picture
        /// </summary>
        /// <param name="notePicture">Note picture</param>
        public virtual async Task UpdateNotePicture(NotePicture notePicture)
        {
            if (notePicture == null)
                throw new ArgumentNullException("notePicture");

            var builder = Builders<Note>.Filter;
            var filter = builder.Eq(x => x.Id, notePicture.NoteId);
            filter = filter & builder.ElemMatch(x => x.NotePictures, y => y.Id == notePicture.Id);
            var update = Builders<Note>.Update
                .Set(x => x.NotePictures.ElementAt(-1).DisplayOrder, notePicture.DisplayOrder)
                .Set(x => x.NotePictures.ElementAt(-1).MimeType, notePicture.MimeType)
                .Set(x => x.NotePictures.ElementAt(-1).SeoFilename, notePicture.SeoFilename)
                .Set(x => x.NotePictures.ElementAt(-1).AltAttribute, notePicture.AltAttribute)
                .Set(x => x.NotePictures.ElementAt(-1).TitleAttribute, notePicture.TitleAttribute);

            await _noteRepository.Collection.UpdateManyAsync(filter, update);

            //cache
            await _cacheManager.RemoveAsync(string.Format(PRODUCTS_BY_ID_KEY, notePicture.NoteId));

            //event notification
            await _mediator.EntityUpdated(notePicture);
        }

        #endregion

        #region Suggested notes

        /// <summary>
        /// Gets suggested notes for user tags
        /// </summary>
        /// <param name="userTagIds">User role ids</param>
        /// <returns>Notes</returns>
        public virtual async Task<IList<Note>> GetSuggestedNotes(string[] userTagIds)
        {
            return await _cacheManager.GetAsync(string.Format(PRODUCTS_CUSTOMER_TAG, string.Join(",", userTagIds)), async () =>
            {
                var query = from cr in _userTagNoteRepository.Table
                            where userTagIds.Contains(cr.UserTagId)
                            orderby cr.DisplayOrder
                            select cr.NoteId;

                var noteIds = await query.ToListAsync();

                var notes = new List<Note>();
                var ids = await GetNotesByIds(noteIds.Distinct().ToArray());
                foreach (var note in ids)
                    notes.Add(note);

                return notes;
            });
        }

        #endregion

        #endregion
    }
}
