namespace ForeverNote.Core.Domain.Notes
{
    /// <summary>
    /// Represents the note sorting
    /// </summary>
    public enum NoteSortingEnum
    {
        /// <summary>
        /// Position (display order)
        /// </summary>
        Position = 0,
        /// <summary>
        /// Name: A to Z
        /// </summary>
        NameAsc = 5,
        /// <summary>
        /// Name: Z to A
        /// </summary>
        NameDesc = 6,
        /// <summary>
        /// Note creation date
        /// </summary>
        CreatedOn = 15,
        /// <summary>
        /// Note most viewed
        /// </summary>
        MostViewed = 25,
        /// <summary>
        /// Note most onsale
        /// </summary>
        OnSale = 30,

    }
}