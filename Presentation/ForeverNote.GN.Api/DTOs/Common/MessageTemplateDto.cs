using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Api.DTOs.Common
{
    public partial class MessageTemplateDto : BaseApiEntityModel
    {
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public string ViewPath { get; set; }
    }
}
