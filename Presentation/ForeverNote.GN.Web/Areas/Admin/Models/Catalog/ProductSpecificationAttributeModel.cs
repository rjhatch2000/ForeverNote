using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Catalog;

namespace ForeverNote.Web.Areas.Admin.Models.Catalog
{
    [Validator(typeof(ProductSpecificationAttributeModelValidator))]
    public partial class ProductSpecificationAttributeModel : BaseForeverNoteEntityModel
    {
        public int AttributeTypeId { get; set; }
        
        public string AttributeTypeName { get; set; }
        
        public string AttributeName { get; set; }

        public string AttributeId { get; set; }
        
        public string ValueRaw { get; set; }

        public bool AllowFiltering { get; set; }

        public bool ShowOnProductPage { get; set; }

        public int DisplayOrder { get; set; }

        public string ProductSpecificationId { get; set; }

        public string SpecificationAttributeOptionId { get; set; }

        public string ProductId { get; set; }
    }
}