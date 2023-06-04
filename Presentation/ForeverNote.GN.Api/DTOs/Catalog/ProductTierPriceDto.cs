using FluentValidation.Attributes;
using ForeverNote.Api.Validators.Catalog;
using ForeverNote.Web.Framework.Mvc.Models;
using System;

namespace ForeverNote.Api.DTOs.Catalog
{
    [Validator(typeof(ProductTierPriceValidator))]
    public partial class ProductTierPriceDto : BaseApiEntityModel
    {
        public string StoreId { get; set; }
        public string CustomerRoleId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime? StartDateTimeUtc { get; set; }
        public DateTime? EndDateTimeUtc { get; set; }
    }
}
