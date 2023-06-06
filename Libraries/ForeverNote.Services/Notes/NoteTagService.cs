using ForeverNote.Core.Caching;
using ForeverNote.Core.Data;
using ForeverNote.Services.Events;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using MediatR;
using ForeverNote.Core.Domain.Notes;

namespace ForeverNote.Services.Notes
{
    /// <summary>
    /// Note tag service
    /// </summary>
    public partial class NoteTagService : INoteTagService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// </remarks>
        private const string PRODUCTTAG_COUNT_KEY = "ForeverNote.notetag.count";

        /// <summary>
        /// Key for all tags
        /// </summary>
        private const string PRODUCTTAG_ALL_KEY = "ForeverNote.notetag.all";


        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTTAG_PATTERN_KEY = "ForeverNote.notetag.";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>        
        private const string PRODUCTS_PATTERN_KEY = "ForeverNote.note.";

        #endregion

        #region Fields

        private readonly IRepository<NoteTag> _noteTagRepository;
        private readonly IRepository<Note> _noteRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMediator _mediator;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="noteTagRepository">Note tag repository</param>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="mediator">Mediator</param>
        public NoteTagService(IRepository<NoteTag> noteTagRepository,
            IRepository<Note> noteRepository,
            ICacheManager cacheManager,
            IMediator mediator
            )
        {
            _noteTagRepository = noteTagRepository;
            _cacheManager = cacheManager;
            _mediator = mediator;
            _noteRepository = noteRepository;
        }

        #endregion

        #region Nested classes

        private class NoteTagWithCount
        {
            public int NoteTagId { get; set; }
            public int NoteCount { get; set; }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get note count for each of existing note tag
        /// </summary>
        /// <returns>Dictionary of "note tag ID : note count"</returns>
        private async Task<Dictionary<string, int>> GetNoteCount()
        {
            string key = string.Format(PRODUCTTAG_COUNT_KEY);
            return await _cacheManager.GetAsync(key, async () =>
             {
                 var query = from pt in _noteTagRepository.Table
                             select pt;

                 var dictionary = new Dictionary<string, int>();
                 foreach (var item in await query.ToListAsync())
                     dictionary.Add(item.Id, item.Count);
                 return dictionary;
             });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a note tag
        /// </summary>
        /// <param name="noteTag">Note tag</param>
        public virtual async Task DeleteNoteTag(NoteTag noteTag)
        {
            if (noteTag == null)
                throw new ArgumentNullException("noteTag");

            var builder = Builders<Note>.Update;
            var updatefilter = builder.Pull(x => x.NoteTags, noteTag.Name);
            await _noteRepository.Collection.UpdateManyAsync(new BsonDocument(), updatefilter);

            await _noteTagRepository.DeleteAsync(noteTag);

            //cache
            await _cacheManager.RemoveByPrefix(PRODUCTTAG_PATTERN_KEY);
            await _cacheManager.RemoveByPrefix(PRODUCTS_PATTERN_KEY);

            //event notification
            await _mediator.EntityDeleted(noteTag);
        }

        /// <summary>
        /// Gets all note tags
        /// </summary>
        /// <returns>Note tags</returns>
        public virtual async Task<IList<NoteTag>> GetAllNoteTags()
        {
            return await _cacheManager.GetAsync(PRODUCTTAG_ALL_KEY, async () =>
            {
                var query = _noteTagRepository.Table;
                return await query.ToListAsync();
            });
        }

        /// <summary>
        /// Gets note tag
        /// </summary>
        /// <param name="noteTagId">Note tag identifier</param>
        /// <returns>Note tag</returns>
        public virtual Task<NoteTag> GetNoteTagById(string noteTagId)
        {
            return _noteTagRepository.GetByIdAsync(noteTagId);
        }

        /// <summary>
        /// Gets note tag by name
        /// </summary>
        /// <param name="name">Note tag name</param>
        /// <returns>Note tag</returns>
        public virtual Task<NoteTag> GetNoteTagByName(string name)
        {
            var query = from pt in _noteTagRepository.Table
                        where pt.Name == name
                        select pt;

            return query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Inserts a note tag
        /// </summary>
        /// <param name="noteTag">Note tag</param>
        public virtual async Task InsertNoteTag(NoteTag noteTag)
        {
            if (noteTag == null)
                throw new ArgumentNullException("noteTag");

            await _noteTagRepository.InsertAsync(noteTag);

            //cache
            await _cacheManager.RemoveByPrefix(PRODUCTTAG_PATTERN_KEY);

            //event notification
            await _mediator.EntityInserted(noteTag);
        }

        /// <summary>
        /// Inserts a note tag
        /// </summary>
        /// <param name="noteTag">Note tag</param>
        public virtual async Task UpdateNoteTag(NoteTag noteTag)
        {
            if (noteTag == null)
                throw new ArgumentNullException("noteTag");

            var previouse = await GetNoteTagById(noteTag.Id);

            await _noteTagRepository.UpdateAsync(noteTag);

            //update name on notes
            var filter = new BsonDocument
            {
                new BsonElement("NoteTags", previouse.Name)
            };
            var update = Builders<Note>.Update
                .Set(x => x.NoteTags.ElementAt(-1), noteTag.Name);
            await _noteRepository.Collection.UpdateManyAsync(filter, update);

            //cache
            await _cacheManager.RemoveByPrefix(PRODUCTTAG_PATTERN_KEY);

            //event notification
            await _mediator.EntityUpdated(noteTag);
        }

        /// <summary>
        /// Get number of notes
        /// </summary>
        /// <param name="noteTagId">Note tag identifier</param>
        /// <returns>Number of notes</returns>
        public virtual async Task<int> GetNoteCount(string noteTagId)
        {
            var dictionary = await GetNoteCount();
            if (dictionary.ContainsKey(noteTagId))
                return dictionary[noteTagId];

            return 0;
        }

        #endregion
    }
}
