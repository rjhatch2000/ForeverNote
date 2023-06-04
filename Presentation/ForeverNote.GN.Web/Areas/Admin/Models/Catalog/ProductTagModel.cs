using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Catalog;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Catalog
{
    [Validator(typeof(ProductTagValidator))]
    public partial class ProductTagModel : BaseForeverNoteEntityModel, ILocalizedModel<ProductTagLocalizedModel>
    {
        public ProductTagModel()
        {
            Locales = new List<ProductTagLocalizedModel>();
        }
        [ForeverNoteResourceDisplayName("Admin.Catalog.ProductTags.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.ProductTags.Fields.ProductCount")]
        public int ProductCount { get; set; }

        public IList<ProductTagLocalizedModel> Locales { get; set; }
    }

    public partial class ProductTagLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.ProductTags.Fields.Name")]
        
        public string Name { get; set; }
    }
}