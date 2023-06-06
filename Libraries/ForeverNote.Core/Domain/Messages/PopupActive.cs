namespace ForeverNote.Core.Domain.Messages
{
    public partial class PopupActive : BaseEntity
    {
        public string Name { get; set; }
        public string Body { get; set; }
        public string UserId { get; set; }
        public string UserActionId { get; set; }
        public int PopupTypeId { get; set; }
    }

}
