using ForeverNote.Core.Configuration;
using System.Collections.Generic;

namespace ForeverNote.Core.Domain.Catalog
{
    public class CatalogSettings : ISettings
    {
        public CatalogSettings()
        {
            NoteSortingEnumDisabled = new List<int>();
            NoteSortingEnumDisplayOrder= new Dictionary<int, int>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether note sorting is enabled
        /// </summary>
        public bool AllowNoteSorting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether users are allowed to change note view mode
        /// </summary>
        public string DefaultViewMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a notebook details page should include notes from subnotebook
        /// </summary>
        public bool ShowNotesFromSubnotebooks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a search box  should include notes from subnotebook
        /// </summary>
        public bool ShowNotesFromSubnotebookInSearchBox { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether number of notes should be displayed beside each notebook
        /// </summary>
        public bool ShowNotebookNoteCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we include subnotebook (when 'ShowNotebookNoteNumber' is 'true')
        /// </summary>
        public bool ShowNotebookNoteNumberIncludingSubnotebooks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether notebook breadcrumb is enabled
        /// </summary>
        public bool NotebookBreadcrumbEnabled { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether a 'Share button' is enabled
        /// </summary>
        public bool ShowShareButton { get; set; }

        //TODO: keep the recently viewed?
        /// <summary>
        /// Gets or sets a number of "Recently viewed notes"
        /// </summary>
        public int RecentlyViewedNotesNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether "Recently viewed notes" feature is enabled
        /// </summary>
        public bool RecentlyViewedNotesEnabled { get; set; }

        /// <summary>
        /// Gets or sets a number of notes on the "New notes" page
        /// </summary>
        public int NewNotesNumber { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether "New notes" page is enabled
        /// </summary>
        public bool NewNotesEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether "New notes" is show on home page
        /// </summary>
        public bool NewNotesOnHomePage { get; set; }
        
        /// <summary>
        /// Gets or sets a number of notes on the "New notes" on home page
        /// </summary>
        public int NewNotesNumberOnHomePage { get; set; }

        //TODO: Think about this one...
        /// <summary>
        /// Gets or sets a value indicating whether autocomplete is enabled
        /// </summary>
        public bool NoteSearchAutoCompleteEnabled { get; set; }

        //TODO: Maybe this is an "only search titles"
        /// <summary>
        /// Gets or sets a value indicating whether search by title is enabled
        /// </summary>
        public bool SearchByTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether search by body is enabled
        /// </summary>
        public bool SearchByBody { get; set; }

        /// <summary>
        /// Gets or sets a number of notes to return when using "autocomplete" feature
        /// </summary>
        public int NoteSearchAutoCompleteNumberOfNotes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show note images in the auto complete search
        /// </summary>
        public bool ShowNoteImagesInSearchAutoComplete { get; set; }

       
        /// <summary>
        /// Gets or sets a minimum search term length
        /// </summary>
        public int NoteSearchTermMinimumLength { get; set; }

        /// <summary>
        /// Gets or sets save search autocomplete
        /// </summary>
        public bool SaveSearchAutoComplete { get; set; }
        
        /// <summary>
        /// Gets or sets a number of notes per page on the search notes page
        /// </summary>
        public int SearchPageNotesPerPage { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether users are allowed to select page size on the search notes page
        /// </summary>
        public bool SearchPageAllowUsersToSelectPageSize { get; set; }
        
        /// <summary>
        /// Gets or sets the available user selectable page size options on the search notes page
        /// </summary>
        public string SearchPagePageSizeOptions { get; set; }

        /// <summary>
        /// Gets or sets a number of note tags that appear in the tag cloud
        /// </summary>
        public int NumberOfNoteTags { get; set; }

        /// <summary>
        /// Gets or sets a number of notes per page on 'notes by tag' page
        /// </summary>
        public int NotesByTagPageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether users can select the page size for 'notes by tag'
        /// </summary>
        public bool NotesByTagAllowUsersToSelectPageSize { get; set; }

        /// <summary>
        /// Gets or sets the available user selectable page size options for 'notes by tag'
        /// </summary>
        public string NotesByTagPageSizeOptions { get; set; }

        /// <summary>
        /// An option indicating whether notes on notebook pages should include featured notes as well
        /// </summary>
        public bool IncludeFeaturedNotesInNormalLists { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to ignore featured notes (side-wide). It can significantly improve performance when enabled.
        /// </summary>
        public bool IgnoreFeaturedNotes { get; set; }

        /// <summary>
        /// Gets or sets the default value to use for Notebook page size options (for new notebook)
        /// </summary>
        public string DefaultNotebookPageSizeOptions { get; set; }
        
        /// <summary>
        /// Gets or sets the default value to use for Notebook page size (for new notebook)
        /// </summary>
        public int DefaultNotebookPageSize { get; set; }
        
        /// <summary>
        /// Limit of featured notes
        /// </summary>
        public int LimitOfFeaturedNotes { get; set; }
        
        /// <summary>
        /// Gets or sets a list of disabled values of NoteSortingEnum
        /// </summary>
        public List<int> NoteSortingEnumDisabled { get; set; }

        /// <summary>
        /// Gets or sets a display order of NoteSortingEnum values 
        /// </summary>
        public Dictionary<int, int> NoteSortingEnumDisplayOrder { get; set; }
    }
}