﻿using FluentValidation.Attributes;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mapping;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Messages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Messages
{
    [Validator(typeof(ContactAttributeValidator))]
    public partial class ContactAttributeModel : BaseForeverNoteEntityModel, ILocalizedModel<ContactAttributeLocalizedModel>, IAclMappingModel, IStoreMappingModel
    {
        public ContactAttributeModel()
        {
            Locales = new List<ContactAttributeLocalizedModel>();
            AvailableCustomerRoles = new List<CustomerRoleModel>();
            AvailableStores = new List<StoreModel>();
        }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.TextPrompt")]
        
        public string TextPrompt { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.IsRequired")]
        public bool IsRequired { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.AttributeControlType")]
        public int AttributeControlTypeId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.AttributeControlType")]
        
        public string AttributeControlTypeName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }


        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.MinLength")]
        [UIHint("Int32Nullable")]
        public int? ValidationMinLength { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.MaxLength")]
        [UIHint("Int32Nullable")]
        public int? ValidationMaxLength { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.FileAllowedExtensions")]
        public string ValidationFileAllowedExtensions { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.FileMaximumSize")]
        [UIHint("Int32Nullable")]
        public int? ValidationFileMaximumSize { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.DefaultValue")]
        public string DefaultValue { get; set; }

        public IList<ContactAttributeLocalizedModel> Locales { get; set; }

        //condition
        public bool ConditionAllowed { get; set; }
        public ConditionModel ConditionModel { get; set; }

        //Store mapping
        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }

        //ACL
        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.SubjectToAcl")]
        public bool SubjectToAcl { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.AclCustomerRoles")]
        public List<CustomerRoleModel> AvailableCustomerRoles { get; set; }
        public string[] SelectedCustomerRoleIds { get; set; }
    }

    public partial class ConditionModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Condition.EnableCondition")]
        public bool EnableCondition { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Condition.Attributes")]
        public string SelectedAttributeId { get; set; }

        public IList<AttributeConditionModel> ConditionAttributes { get; set; }
    }
    public partial class AttributeConditionModel : BaseForeverNoteEntityModel
    {
        public string Name { get; set; }

        public AttributeControlType AttributeControlType { get; set; }

        public IList<SelectListItem> Values { get; set; }

        public string SelectedValueId { get; set; }
    }
    public partial class ContactAttributeLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Catalog.Attributes.ContactAttributes.Fields.TextPrompt")]
        
        public string TextPrompt { get; set; }

    }
}