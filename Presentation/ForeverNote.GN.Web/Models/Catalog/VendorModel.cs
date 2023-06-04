using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Models.Media;
using ForeverNote.Web.Models.Vendors;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Catalog
{
    public partial class VendorModel : BaseForeverNoteEntityModel
    {
        public VendorModel()
        {
            Products = new List<ProductOverviewModel>();
            PagingFilteringContext = new CatalogPagingFilteringModel();
            Address = new VendorAddressModel();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }
        public bool AllowCustomersToContactVendors { get; set; }
        public VendorAddressModel Address { get; set; }
        public PictureModel PictureModel { get; set; }
        public CatalogPagingFilteringModel PagingFilteringContext { get; set; }
        public VendorReviewOverviewModel VendorReviewOverview { get; set; }
        public IList<ProductOverviewModel> Products { get; set; }
    }
}