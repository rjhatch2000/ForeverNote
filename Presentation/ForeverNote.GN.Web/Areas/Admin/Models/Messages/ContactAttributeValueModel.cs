using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Messages;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Messages
{
    [Validator(typeof(ContactAttributeValueValidator))]
    public partial class ContactAttributeValueModel : BaseForeverNoteEntityModel, ILocalizedModel<ContactAttributeValueLocalizedModel>
    {
        public ContactAttributeValueModel()
        {
            Locales = new List<ContactAttributeValueLocalizedModel>();
        }

        public string ContactAttributeId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Values.Fields.Name")]
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Values.Fields.ColorSquaresRgb")]
        public string ColorSquaresRgb { get; set; }
        public bool DisplayColorSquaresRgb { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Values.Fields.IsPreSelected")]
        public bool IsPreSelected { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Values.Fields.DisplayOrder")]
        public int DisplayOrder {get;set;}

        public IList<ContactAttributeValueLocalizedModel> Locales { get; set; }

    }

    public partial class ContactAttributeValueLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Values.Fields.Name")]
        public string Name { get; set; }
    }
}