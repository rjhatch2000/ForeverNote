using ForeverNote.Core.Configuration;

namespace ForeverNote.Core.Domain.AdminSearch
{
    public class AdminSearchSettings : ISettings
    {
        public bool SearchInNotes { get; set; }

        public bool SearchInNotebooks { get; set; }

        public int MinSearchTermLength { get; set; }

        public int MaxSearchResultsCount { get; set; }

        public int NotesDisplayOrder { get; set; }

        public int NotebooksDisplayOrder { get; set; }

        public bool SearchInMenu { get; set; }

        public int MenuDisplayOrder { get; set; }
    }
}
