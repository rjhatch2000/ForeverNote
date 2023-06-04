using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Stores;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Stores
{
    [Validator(typeof(StoreValidator))]
    public partial class StoreModel : BaseForeverNoteEntityModel, ILocalizedModel<StoreLocalizedModel>
    {
        public StoreModel()
        {
            Locales = new List<StoreLocalizedModel>();
            AvailableLanguages = new List<SelectListItem>();
            AvailableWarehouses = new List<SelectListItem>();
            AvailableCountries = new List<SelectListItem>();
        }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.Name")]
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.Shortcut")]
        public string Shortcut { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.Url")]        
        public string Url { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.SslEnabled")]
        public virtual bool SslEnabled { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.SecureUrl")]        
        public virtual string SecureUrl { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.Hosts")]        
        public string Hosts { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyName")]        
        public string CompanyName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyAddress")]        
        public string CompanyAddress { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyPhoneNumber")]        
        public string CompanyPhoneNumber { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyVat")]        
        public string CompanyVat { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyEmail")]
        public string CompanyEmail { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyHours")]
        public string CompanyHours { get; set; }

        public IList<StoreLocalizedModel> Locales { get; set; }
        //default language
        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultLanguage")]        
        public string DefaultLanguageId { get; set; }
        public IList<SelectListItem> AvailableLanguages { get; set; }

        //default warehouse
        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultWarehouse")]        
        public string DefaultWarehouseId { get; set; }
        public IList<SelectListItem> AvailableWarehouses { get; set; }

        //default country
        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultCountry")]
        public string DefaultCountryId { get; set; }
        public IList<SelectListItem> AvailableCountries { get; set; }

    }

    public partial class StoreLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.Name")]
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Stores.Fields.Shortcut")]
        public string Shortcut { get; set; }
    }
}