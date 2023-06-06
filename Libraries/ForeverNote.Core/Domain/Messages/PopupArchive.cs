using System;

namespace ForeverNote.Core.Domain.Messages
{
    public partial class PopupArchive : BaseEntity
    {
        public string PopupActiveId { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string UserId { get; set; }
        public string UserActionId { get; set; }
        public int PopupTypeId { get; set; }
        public DateTime BACreatedOnUtc { get; set; }
    }
}
