﻿using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Api.DTOs.Shipping
{
    public partial class DeliveryDateDto : BaseApiEntityModel
    {
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public string ColorSquaresRgb { get; set; }
    }
}
