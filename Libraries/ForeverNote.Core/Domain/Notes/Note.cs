using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Localization;
using System;
using System.Collections.Generic;

namespace ForeverNote.Core.Domain.Notes
{
    /// <summary>
    /// Represents a note
    /// </summary>
    public partial class Note : BaseEntity, ILocalizedEntity
    {
        private ICollection<NoteNotebook> _noteNotebooks;
        private ICollection<NotePicture> _notePictures;
        private ICollection<string> _noteTags;
        public Note()
        {
            UserRoles = new List<string>();
            Locales = new List<LocalizedProperty>();
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        ////public string SeName { get; set; } //TODO: Delete all SeName items
        /// <summary>
        /// Gets or sets the short description
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the full description
        /// </summary>
        public string FullDescription { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the note on home page
        /// </summary>
        public bool ShowOnHomePage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the last viewed date
        /// </summary>
        public DateTime LastViewedOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is subject to ACL
        /// </summary>
        public bool SubjectToAcl { get; set; }
        public IList<string> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the ExternalId
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the note is recurring
        /// </summary>
        public bool IsRecurring { get; set; }
        /// <summary>
        /// Gets or sets the cycle length
        /// </summary>
        public int RecurringCycleLength { get; set; }
        /// <summary>
        /// Gets or sets the cycle period
        /// </summary>
        public int RecurringCyclePeriodId { get; set; }
        /// <summary>
        /// Gets or sets the total cycles
        /// </summary>
        public int RecurringTotalCycles { get; set; }
        /// <summary>
        /// Gets or sets include both dates
        /// </summary>
        public bool IncBothDate { get; set; }
        /// <summary>
        /// Gets or sets Interval
        /// </summary>
        public int Interval { get; set; }
        /// <summary>
        /// Gets or sets IntervalUnitId
        /// </summary>
        public int IntervalUnitId { get; set; }
        /// <summary>
        /// Gets or sets Interval Unit
        /// </summary>
        public IntervalUnit IntervalUnitType
        {
            get
            {
                return (IntervalUnit)IntervalUnitId;
            }
            set
            {
                IntervalUnitId = (int)value;
            }

        }
        /// <summary>
        /// Gets or sets a delivery date identifier
        /// </summary>
        public string DeliveryDateId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to back in stock subscriptions are allowed
        /// </summary>
        public bool AllowBackInStockSubscriptions { get; set; }

        /// <summary>
        /// Gets or sets a unit of note 
        /// </summary>
        public string UnitId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this note is marked as new
        /// </summary>
        public bool MarkAsNew { get; set; }
        /// <summary>
        /// Gets or sets the start date and time of the new note (set note as "New" from date). Leave empty to ignore this property
        /// </summary>
        public DateTime? MarkAsNewStartDateTimeUtc { get; set; }
        /// <summary>
        /// Gets or sets the end date and time of the new note (set note as "New" to date). Leave empty to ignore this property
        /// </summary>
        public DateTime? MarkAsNewEndDateTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets a display order.
        /// This value is used when sorting associated notes (used with "grouped" notes)
        /// This value is used when sorting home page notes
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets a display order for notebook page.
        /// This value is used when sorting notes on notebook page
        /// </summary>
        public int DisplayOrderNotebook { get; set; }

        /// <summary>
        /// Gets or sets the viewed
        /// </summary>
        public long Viewed { get; set; }

        /// <summary>
        /// Gets or sets the onsale
        /// </summary>
        public int OnSale { get; set; }

        /// <summary>
        /// Gets or sets the flag
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// Gets or sets the collection of locales
        /// </summary>
        public IList<LocalizedProperty> Locales { get; set; }

        /// <summary>
        /// Gets or sets the cycle period for recurring notes
        /// </summary>
        public RecurringNoteCyclePeriod RecurringCyclePeriod
        {
            get
            {
                return (RecurringNoteCyclePeriod)RecurringCyclePeriodId;
            }
            set
            {
                RecurringCyclePeriodId = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of NoteNotebook
        /// </summary>
        public virtual ICollection<NoteNotebook> NoteNotebooks
        {
            get { return _noteNotebooks ?? (_noteNotebooks = new List<NoteNotebook>()); }
            protected set { _noteNotebooks = value; }
        }

        /// <summary>
        /// Gets or sets the collection of NotePicture
        /// </summary>
        public virtual ICollection<NotePicture> NotePictures
        {
            get { return _notePictures ?? (_notePictures = new List<NotePicture>()); }
            protected set { _notePictures = value; }
        }

        /// <summary>
        /// Gets or sets the note tags
        /// </summary>
        public virtual ICollection<string> NoteTags
        {
            get { return _noteTags ?? (_noteTags = new List<string>()); }
            protected set { _noteTags = value; }
        }
    }
}