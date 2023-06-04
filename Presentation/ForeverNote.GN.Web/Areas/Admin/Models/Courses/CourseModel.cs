using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mapping;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Courses
{
    [Validator(typeof(CourseValidator))]
    public partial class CourseModel : BaseForeverNoteEntityModel, ILocalizedModel<CourseLocalizedModel>, IAclMappingModel, IStoreMappingModel
    {
        public CourseModel()
        {
            Locales = new List<CourseLocalizedModel>();
            AvailableLevels = new List<SelectListItem>();
        }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.Name")]
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.ShortDescription")]
        public string ShortDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.Description")]
        public string Description { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.Published")]
        public bool Published { get; set; }

        [UIHint("Picture")]
        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.PictureId")]
        public string PictureId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.LevelId")]
        public string LevelId { get; set; }
        public IList<SelectListItem> AvailableLevels { get; set; }

        //ACL
        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.SubjectToAcl")]
        public bool SubjectToAcl { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.AclCustomerRoles")]
        public List<CustomerRoleModel> AvailableCustomerRoles { get; set; }
        public string[] SelectedCustomerRoleIds { get; set; }

        //Store mapping
        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.MetaDescription")]

        public string MetaDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.MetaTitle")]

        public string MetaTitle { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.SeName")]

        public string SeName { get; set; }

        public IList<CourseLocalizedModel> Locales { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.ProductId")]
        public string ProductId { get; set; }
        public string ProductName { get; set; }

        #region Nested classes

        public partial class AssociateProductToCourseModel : BaseForeverNoteModel
        {
            public AssociateProductToCourseModel()
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

            //vendor
            public bool IsLoggedInAsVendor { get; set; }


            public string AssociatedToProductId { get; set; }
        }
        #endregion
    }

    public partial class CourseLocalizedModel : ILocalizedModelLocal, ISlugModelLocal
    {
        public string LanguageId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.Name")]

        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.ShortDescription")]

        public string ShortDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.Description")]

        public string Description { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.MetaKeywords")]

        public string MetaKeywords { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.MetaDescription")]

        public string MetaDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.MetaTitle")]

        public string MetaTitle { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Courses.Course.Fields.SeName")]

        public string SeName { get; set; }

    }
}
