using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Blogs
{
    public partial class BlogProductModel : BaseForeverNoteEntityModel
    {
        public string ProductId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.Product.Fields.ProductName")]
        public string ProductName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Blog.Product.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public partial class AddProductModel : BaseForeverNoteModel
        {
            public AddProductModel()
            {
                AvailableCategories = new List<SelectListItem>();
                AvailableManufacturers = new List<SelectListItem>();
                AvailableStores = new List<SelectListItem>();
                AvailableVendors = new List<SelectListItem>();
                AvailableProductTypes = new List<SelectListItem>();
            }

            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]

            public string SearchProductName { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
            public string SearchCategoryId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
            public string SearchManufacturerId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
            public string SearchStoreId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchVendor")]
            public string SearchVendorId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Catalog.Products.List.SearchProductType")]
            public int SearchProductTypeId { get; set; }

            public IList<SelectListItem> AvailableCategories { get; set; }
            public IList<SelectListItem> AvailableManufacturers { get; set; }
            public IList<SelectListItem> AvailableStores { get; set; }
            public IList<SelectListItem> AvailableVendors { get; set; }
            public IList<SelectListItem> AvailableProductTypes { get; set; }

            public string BlogPostId { get; set; }

            public string[] SelectedProductIds { get; set; }
        }
    }

}
