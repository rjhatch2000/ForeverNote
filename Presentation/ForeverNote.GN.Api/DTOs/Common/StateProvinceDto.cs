﻿using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Api.DTOs.Common
{
    public partial class StateProvinceDto : BaseApiEntityModel
    {
        public string CountryId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }
    }
}
