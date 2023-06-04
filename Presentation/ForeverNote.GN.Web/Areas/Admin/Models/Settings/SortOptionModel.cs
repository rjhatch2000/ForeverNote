using ForeverNote.Web.Framework.Mvc.ModelBinding;

namespace ForeverNote.Web.Areas.Admin.Models.Settings
{
    public partial class SortOptionModel
    {
        public virtual int Id { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.Catalog.SortOptions.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.Catalog.SortOptions.IsActive")]        
        public bool IsActive { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.Catalog.SortOptions.DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
}