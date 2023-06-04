using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Framework.Mvc.Models
{
    public partial class BaseApiEntityModel
    {
        [Key]
        public string Id { get; set; }
    }
}
