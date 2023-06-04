using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mapping;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Localization
{
    [Validator(typeof(LanguageValidator))]
    public partial class LanguageModel : BaseForeverNoteEntityModel, IStoreMappingModel
    {
        public LanguageModel()
        {
            FlagFileNames = new List<string>();
            AvailableCurrencies = new List<SelectListItem>();
            Search = new LanguageResourceFilterModel();
            AvailableStores = new List<StoreModel>();
        }
        [ForeverNoteResourceDisplayName("Admin.Configuration.Languages.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Languages.Fields.LanguageCulture")]
        
        public string LanguageCulture { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Languages.Fields.UniqueSeoCode")]
        
        public string UniqueSeoCode { get; set; }
        
        //flags
        [ForeverNoteResourceDisplayName("Admin.Configuration.Languages.Fields.FlagImageFileName")]
        
        public string FlagImageFileName { get; set; }
        public IList<string> FlagFileNames { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Languages.Fields.Rtl")]
        public bool Rtl { get; set; }

        //default currency
        [ForeverNoteResourceDisplayName("Admin.Configuration.Languages.Fields.DefaultCurrency")]
        
        public string DefaultCurrencyId { get; set; }
        public IList<SelectListItem> AvailableCurrencies { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Languages.Fields.Published")]
        public bool Published { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Languages.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }


        //Store mapping
        [ForeverNoteResourceDisplayName("Admin.Configuration.Languages.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Configuration.Languages.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }

        public LanguageResourceFilterModel Search { get; set; }
    }
}