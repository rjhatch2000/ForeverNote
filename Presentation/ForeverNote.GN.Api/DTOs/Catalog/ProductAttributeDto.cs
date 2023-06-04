using FluentValidation.Attributes;
using ForeverNote.Api.Validators.Catalog;
using ForeverNote.Web.Framework.Mvc.Models;
using System.Collections.Generic;

namespace ForeverNote.Api.DTOs.Catalog
{
    [Validator(typeof(ProductAttributeValidator))]
    public partial class ProductAttributeDto : BaseApiEntityModel
    {
        public ProductAttributeDto()
        {
            PredefinedProductAttributeValues = new List<PredefinedProductAttributeValueDto>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<PredefinedProductAttributeValueDto> PredefinedProductAttributeValues { get; set; }
    }

    public partial class PredefinedProductAttributeValueDto: BaseApiEntityModel
    {
        public string Name { get; set; }
        public decimal PriceAdjustment { get; set; }
        public decimal WeightAdjustment { get; set; }
        public decimal Cost { get; set; }
        public bool IsPreSelected { get; set; }
        public int DisplayOrder { get; set; }
    }
}
