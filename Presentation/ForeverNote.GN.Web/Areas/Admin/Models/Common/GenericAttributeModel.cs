﻿using FluentValidation.Attributes;
using ForeverNote.Web.Areas.Admin.Validators.Common;

namespace ForeverNote.Web.Areas.Admin.Models.Common
{
    [Validator(typeof(GenericAttributeValidator))]
    public partial class GenericAttributeModel
    {
        public string Id { get; set; }
        public string ObjectType { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string StoreId { get; set; }
        public int SelectedTab { get; set; }
    }
}