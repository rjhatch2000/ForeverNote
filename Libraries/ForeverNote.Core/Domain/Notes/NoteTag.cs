using ForeverNote.Core.Domain.Localization;
using System.Collections.Generic;

namespace ForeverNote.Core.Domain.Notes
{
    /// <summary>
    /// Represents a note tag
    /// </summary>
    public partial class NoteTag : BaseEntity, ILocalizedEntity
    {
        public NoteTag()
        {
            Locales = new List<LocalizedProperty>();
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets note id
        /// </summary>
        public string NoteId { get; set; }

        /// <summary>
        /// Gets or sets the collection of locales
        /// </summary>
        public IList<LocalizedProperty> Locales { get; set; }
    }
}
