using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Settings;

namespace ForeverNote.Web.Areas.Admin.Models.Settings
{
    [Validator(typeof(SettingValidator))]
    public partial class SettingModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.Value")]
        
        public string Value { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.StoreName")]
        public string Store { get; set; }
        public string StoreId { get; set; }
    }
}