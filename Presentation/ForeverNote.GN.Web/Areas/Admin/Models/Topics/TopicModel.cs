using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mapping;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Topics;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Topics
{
    [Validator(typeof(TopicValidator))]
    public partial class TopicModel : BaseForeverNoteEntityModel, ILocalizedModel<TopicLocalizedModel>, IAclMappingModel, IStoreMappingModel
    {
        public TopicModel()
        {
            AvailableTopicTemplates = new List<SelectListItem>();
            Locales = new List<TopicLocalizedModel>();
            AvailableStores = new List<StoreModel>();
            AvailableCustomerRoles = new List<CustomerRoleModel>();
        }

        //Store mapping
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }


        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.SystemName")]
        
        public string SystemName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInSitemap")]
        public bool IncludeInSitemap { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInTopMenu")]
        public bool IncludeInTopMenu { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInFooterRow1")]
        public bool IncludeInFooterRow1 { get; set; }
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInFooterRow2")]
        public bool IncludeInFooterRow2 { get; set; }
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInFooterRow3")]
        public bool IncludeInFooterRow3 { get; set; }
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.AccessibleWhenStoreClosed")]
        public bool AccessibleWhenStoreClosed { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.IsPasswordProtected")]
        public bool IsPasswordProtected { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.Published")]
        public bool Published { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.Password")]
        public string Password { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.URL")]
        
        public string Url { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.Title")]
        
        public string Title { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.Body")]
        
        public string Body { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.TopicTemplate")]
        public string TopicTemplateId { get; set; }
        public IList<SelectListItem> AvailableTopicTemplates { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaKeywords")]
        
        public string MetaKeywords { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaDescription")]
        
        public string MetaDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaTitle")]
        
        public string MetaTitle { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.SeName")]
        
        public string SeName { get; set; }
        
        public IList<TopicLocalizedModel> Locales { get; set; }
        //ACL
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.SubjectToAcl")]
        public bool SubjectToAcl { get; set; }
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.AclCustomerRoles")]
        public List<CustomerRoleModel> AvailableCustomerRoles { get; set; }
        public string[] SelectedCustomerRoleIds { get; set; }

    }

    public partial class TopicLocalizedModel : ILocalizedModelLocal, ISlugModelLocal
    {
        public string LanguageId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.Title")]
        
        public string Title { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.Body")]
        
        public string Body { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaKeywords")]
        
        public string MetaKeywords { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaDescription")]
        
        public string MetaDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaTitle")]
        
        public string MetaTitle { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.Fields.SeName")]
        
        public string SeName { get; set; }

    }
}