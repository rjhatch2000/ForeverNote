using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mapping;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Directory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Directory
{
    [Validator(typeof(CurrencyValidator))]
    public partial class CurrencyModel : BaseForeverNoteEntityModel, ILocalizedModel<CurrencyLocalizedModel>, IStoreMappingModel
    {
        public CurrencyModel()
        {
            Locales = new List<CurrencyLocalizedModel>();
            AvailableStores = new List<StoreModel>();
        }
        [ForeverNoteResourceDisplayName("Admin.Configuration.Currencies.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Currencies.Fields.CurrencyCode")]
        
        public string CurrencyCode { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Currencies.Fields.DisplayLocale")]
        
        public string DisplayLocale { get; set; }

        [UIHint("DecimalN4")]
        [ForeverNoteResourceDisplayName("Admin.Configuration.Currencies.Fields.Rate")]
        public decimal Rate { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Currencies.Fields.CustomFormatting")]
        
        public string CustomFormatting { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Currencies.Fields.Published")]
        public bool Published { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Currencies.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Currencies.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Currencies.Fields.IsPrimaryExchangeRateCurrency")]
        public bool IsPrimaryExchangeRateCurrency { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Currencies.Fields.IsPrimaryStoreCurrency")]
        public bool IsPrimaryStoreCurrency { get; set; }

        public IList<CurrencyLocalizedModel> Locales { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Currencies.Fields.RoundingType")]
        public int RoundingTypeId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Currencies.Fields.MidpointRound")]
        public int MidpointRoundId { get; set; }

        //Store mapping
        [ForeverNoteResourceDisplayName("Admin.Configuration.Currencies.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Configuration.Currencies.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }
    }

    public partial class CurrencyLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Currencies.Fields.Name")]
        
        public string Name { get; set; }
    }
}