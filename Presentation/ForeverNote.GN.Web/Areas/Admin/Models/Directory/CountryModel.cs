using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mapping;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Directory;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Directory
{
    [Validator(typeof(CountryValidator))]
    public partial class CountryModel : BaseForeverNoteEntityModel, ILocalizedModel<CountryLocalizedModel>, IStoreMappingModel
    {
        public CountryModel()
        {
            this.AvailableStores = new List<StoreModel>();
            Locales = new List<CountryLocalizedModel>();
        }
        [ForeverNoteResourceDisplayName("Admin.Configuration.Countries.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Countries.Fields.AllowsBilling")]
        public bool AllowsBilling { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Countries.Fields.AllowsShipping")]
        public bool AllowsShipping { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Countries.Fields.TwoLetterIsoCode")]
        
        public string TwoLetterIsoCode { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Countries.Fields.ThreeLetterIsoCode")]
        
        public string ThreeLetterIsoCode { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Countries.Fields.NumericIsoCode")]
        public int NumericIsoCode { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Countries.Fields.SubjectToVat")]
        public bool SubjectToVat { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Countries.Fields.Published")]
        public bool Published { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Countries.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }


        [ForeverNoteResourceDisplayName("Admin.Configuration.Countries.Fields.NumberOfStates")]
        public int NumberOfStates { get; set; }

        public IList<CountryLocalizedModel> Locales { get; set; }


        //Store mapping
        [ForeverNoteResourceDisplayName("Admin.Configuration.Countries.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Configuration.Countries.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }
    }

    public partial class CountryLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Countries.Fields.Name")]
        
        public string Name { get; set; }
    }
}