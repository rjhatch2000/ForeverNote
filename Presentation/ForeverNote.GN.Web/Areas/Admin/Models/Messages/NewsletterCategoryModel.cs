using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mapping;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Messages;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Messages
{
    [Validator(typeof(NewsletterCategoryValidator))]
    public partial class NewsletterCategoryModel: BaseForeverNoteEntityModel, ILocalizedModel<NewsletterCategoryLocalizedModel>, IStoreMappingModel
    {
        public NewsletterCategoryModel()
        {
            Locales = new List<NewsletterCategoryLocalizedModel>();
            AvailableStores = new List<StoreModel>();
        }

        [ForeverNoteResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.Description")]
        
        public string Description { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.Selected")]
        public bool Selected { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        public string[] SelectedStoreIds { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
        public IList<NewsletterCategoryLocalizedModel> Locales { get; set; }
    }

    public partial class NewsletterCategoryLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Promotions.NewsletterCategory.Fields.Description")]
        
        public string Description { get; set; }

    }
}