using FluentValidation.Attributes;
using ForeverNote.Api.Validators.Catalog;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Api.DTOs.Catalog
{
    [Validator(typeof(ProductManufacturerValidator))]
    public partial class ProductManufacturerDto
    {
        [Key]
        public string ManufacturerId { get; set; }
        public bool IsFeaturedProduct { get; set; }
    }
}
