using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Security
{
    public partial class PermissionRecordModel : BaseForeverNoteModel
    {
        public string Name { get; set; }
        public string SystemName { get; set; }
    }
}