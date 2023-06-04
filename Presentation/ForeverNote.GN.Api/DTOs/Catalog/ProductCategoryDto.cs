using FluentValidation.Attributes;
using ForeverNote.Api.Validators.Catalog;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Api.DTOs.Catalog
{
    [Validator(typeof(ProductCategoryValidator))]
    public partial class ProductCategoryDto
    {
        [Key]
        public string CategoryId { get; set; }
        public bool IsFeaturedProduct { get; set; }
    }
}
