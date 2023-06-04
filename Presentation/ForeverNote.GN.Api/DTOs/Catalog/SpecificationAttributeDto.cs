using FluentValidation.Attributes;
using ForeverNote.Api.Validators.Catalog;
using ForeverNote.Web.Framework.Mvc.Models;
using System.Collections.Generic;

namespace ForeverNote.Api.DTOs.Catalog
{
    [Validator(typeof(SpecificationAttributeValidator))]
    public partial class SpecificationAttributeDto: BaseApiEntityModel
    {
        public SpecificationAttributeDto()
        {
            SpecificationAttributeOptions = new List<SpecificationAttributeOptionDto>();
        }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public IList<SpecificationAttributeOptionDto> SpecificationAttributeOptions { get; set; }

    }
    public partial class SpecificationAttributeOptionDto : BaseApiEntityModel
    {
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public string ColorSquaresRgb { get; set; }
    }
}
