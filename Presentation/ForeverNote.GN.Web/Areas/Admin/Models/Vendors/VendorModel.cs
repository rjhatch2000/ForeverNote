using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Models.Common;
using ForeverNote.Web.Areas.Admin.Models.Discounts;
using ForeverNote.Web.Areas.Admin.Validators.Vendors;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Vendors
{
    [Validator(typeof(VendorValidator))]
    public partial class VendorModel : BaseForeverNoteEntityModel, ILocalizedModel<VendorLocalizedModel>
    {
        public VendorModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }
            Locales = new List<VendorLocalizedModel>();
            AssociatedCustomers = new List<AssociatedCustomerInfo>();
            Address = new AddressModel();
            AvailableStores = new List<SelectListItem>();
        }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.Email")]
        
        public string Email { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.Description")]
        
        public string Description { get; set; }

        [UIHint("Picture")]
        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.Picture")]
        public string PictureId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.Store")]
        public string StoreId { get; set; }
        public List<SelectListItem> AvailableStores { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.AdminComment")]
        
        public string AdminComment { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.Active")]
        public bool Active { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.AllowCustomerReviews")]
        public bool AllowCustomerReviews { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.MetaKeywords")]
        
        public string MetaKeywords { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.MetaDescription")]
        
        public string MetaDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.MetaTitle")]
        
        public string MetaTitle { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.SeName")]
        
        public string SeName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.PageSize")]
        public int PageSize { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.AllowCustomersToSelectPageSize")]
        public bool AllowCustomersToSelectPageSize { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.PageSizeOptions")]
        public string PageSizeOptions { get; set; }

        public IList<VendorLocalizedModel> Locales { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.AssociatedCustomerEmails")]
        public IList<AssociatedCustomerInfo> AssociatedCustomers { get; set; }

        public AddressModel Address { get; set; }

        //vendor notes
        [ForeverNoteResourceDisplayName("Admin.Vendors.VendorNotes.Fields.Note")]
        
        public string AddVendorNoteMessage { get; set; }

        public List<DiscountModel> AvailableDiscounts { get; set; }
        public string[] SelectedDiscountIds { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.Commission")]

        public decimal Commission { get; set; } = 0;


        #region Nested classes

        public class AssociatedCustomerInfo : BaseForeverNoteEntityModel
        {
            public string Email { get; set; }
        }


        public partial class VendorNote : BaseForeverNoteEntityModel
        {
            public string VendorId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Vendors.VendorNotes.Fields.Note")]
            public string Note { get; set; }
            [ForeverNoteResourceDisplayName("Admin.Vendors.VendorNotes.Fields.CreatedOn")]
            public DateTime CreatedOn { get; set; }
        }
        #endregion

    }

    public partial class VendorLocalizedModel : ILocalizedModelLocal, ISlugModelLocal
    {
        public string LanguageId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.Description")]
        
        public string Description { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.MetaKeywords")]
        
        public string MetaKeywords { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.MetaDescription")]
        
        public string MetaDescription { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.MetaTitle")]
        
        public string MetaTitle { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Vendors.Fields.SeName")]
        
        public string SeName { get; set; }
    }
}