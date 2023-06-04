using ForeverNote.Web.Framework.Mvc.ModelBinding;

namespace ForeverNote.Web.Areas.Admin.Models.Settings
{
    public class AdminSearchSettingsModel
    {
        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInProducts")]
        public bool SearchInProducts { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.ProductsDisplayOrder")]
        public int ProductsDisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInCategories")]
        public bool SearchInCategories { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.CategoriesDisplayOrder")]
        public int CategoriesDisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInManufacturers")]
        public bool SearchInManufacturers { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.ManufacturersDisplayOrder")]
        public int ManufacturersDisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInTopics")]
        public bool SearchInTopics { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.TopicsDisplayOrder")]
        public int TopicsDisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInNews")]
        public bool SearchInNews { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.NewsDisplayOrder")]
        public int NewsDisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInBlogs")]
        public bool SearchInBlogs { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.BlogsDisplayOrder")]
        public int BlogsDisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInCustomers")]
        public bool SearchInCustomers { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.CustomersDisplayOrder")]
        public int CustomersDisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInOrders")]
        public bool SearchInOrders { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.OrdersDisplayOrder")]
        public int OrdersDisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.MinSearchTermLength")]
        public int MinSearchTermLength { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.MaxSearchResultsCount")]
        public int MaxSearchResultsCount { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.SearchInMenu")]
        public bool SearchInMenu { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Settings.AdminSearch.Fields.MenuDisplayOrder")]
        public int MenuDisplayOrder { get; set; }
    }
}
