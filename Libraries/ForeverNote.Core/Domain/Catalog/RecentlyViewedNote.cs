namespace ForeverNote.Core.Domain.Catalog
{
    public partial class RecentlyViewedNote: BaseEntity
    {
        public string UserId { get; set; }
        public string NoteId { get; set; }
    }
}
