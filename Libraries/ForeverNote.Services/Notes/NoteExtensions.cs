using ForeverNote.Core.Domain.Notes;
using System;
using System.Linq;

namespace ForeverNote.Services.Notes
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class NoteExtensions
    {
        /// <summary>
        /// Indicates whether a note tag exists
        /// </summary>
        /// <param name="note">Note</param>
        /// <param name="noteTagId">Note tag identifier</param>
        /// <returns>Result</returns>
        public static bool NoteTagExists(this Note note,
            string noteTagName)
        {
            if (note == null)
                throw new ArgumentNullException("note");

            bool result = note.NoteTags.FirstOrDefault(pt => pt == noteTagName) != null;
            return result;
        }
    }
}
