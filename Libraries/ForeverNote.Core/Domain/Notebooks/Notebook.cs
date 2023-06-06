using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Localization;
using System.Collections.Generic;

namespace ForeverNote.Core.Domain.Notebooks
{
    /// <summary>
    /// Represents a notebook
    /// </summary>
    public partial class Notebook : BaseEntity, ILocalizedEntity, ITreeNode
    {
        public Notebook()
        {
            Locales = new List<LocalizedProperty>();
        }
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the parent notebook identifier
        /// </summary>
        public string ParentNotebookId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public string PictureId { get; set; }

        /// <summary>
        /// Gets or sets the page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the notebook on home page
        /// </summary>
        public bool ShowOnHomePage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show featured notes on home page
        /// </summary>
        public bool FeaturedNotesOnHomePage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the notebook on search box
        /// </summary>
        public bool ShowOnSearchBox { get; set; }

        /// <summary>
        /// Gets or sets the display order on search box notebook
        /// </summary>
        public int SearchBoxDisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the flag
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// Gets or sets the flag style
        /// </summary>
        public string FlagStyle { get; set; }

        /// <summary>
        /// Gets or sets the Icon
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the Default sort
        /// </summary>
        public int DefaultSort { get; set; } = -1;

        /// <summary>
        /// Gets or sets the hide on catalog page (subnotebook)
        /// </summary>
        public bool HideOnCatalog { get; set; }

        /// <summary>
        /// Gets or sets the collection of locales
        /// </summary>
        public IList<LocalizedProperty> Locales { get; set; }
    }
}
