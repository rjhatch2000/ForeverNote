using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Shipping;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Shipping
{
    [Validator(typeof(ShippingMethodValidator))]
    public partial class ShippingMethodModel : BaseForeverNoteEntityModel, ILocalizedModel<ShippingMethodLocalizedModel>
    {
        public ShippingMethodModel()
        {
            Locales = new List<ShippingMethodLocalizedModel>();
        }
        [ForeverNoteResourceDisplayName("Admin.Configuration.Shipping.Methods.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Shipping.Methods.Fields.Description")]
        
        public string Description { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Shipping.Methods.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<ShippingMethodLocalizedModel> Locales { get; set; }
    }

    public partial class ShippingMethodLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Shipping.Methods.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Shipping.Methods.Fields.Description")]
        
        public string Description { get; set; }

    }
}