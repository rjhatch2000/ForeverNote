using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mapping;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Plugins;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Plugins
{
    [Validator(typeof(PluginValidator))]
    public partial class PluginModel : BaseForeverNoteModel, ILocalizedModel<PluginLocalizedModel>, IStoreMappingModel
    {
        public PluginModel()
        {
            Locales = new List<PluginLocalizedModel>();
            AvailableStores = new List<StoreModel>();
        }
        [ForeverNoteResourceDisplayName("Admin.Configuration.Plugins.Fields.Group")]
        
        public string Group { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Plugins.Fields.FriendlyName")]
        
        public string FriendlyName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Plugins.Fields.SystemName")]
        
        public string SystemName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Plugins.Fields.Version")]
        
        public string Version { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Plugins.Fields.Author")]
        
        public string Author { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Plugins.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Plugins.Fields.Configure")]
        public string ConfigurationUrl { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Plugins.Fields.Installed")]
        public bool Installed { get; set; }

        public bool CanChangeEnabled { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Configuration.Plugins.Fields.IsEnabled")]
        public bool IsEnabled { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Plugins.Fields.Logo")]
        public string LogoUrl { get; set; }

        public IList<PluginLocalizedModel> Locales { get; set; }


        //Store mapping
        [ForeverNoteResourceDisplayName("Admin.Configuration.Plugins.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Configuration.Plugins.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }
    }
    public partial class PluginLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Plugins.Fields.FriendlyName")]
        
        public string FriendlyName { get; set; }
    }
}