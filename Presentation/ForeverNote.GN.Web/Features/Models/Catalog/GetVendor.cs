using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Core.Domain.Vendors;
using ForeverNote.Web.Models.Catalog;
using MediatR;

namespace ForeverNote.Web.Features.Models.Catalog
{
    public class GetVendor : IRequest<VendorModel>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public Language Language { get; set; }
        public Vendor Vendor { get; set; }
        public CatalogPagingFilteringModel Command { get; set; }

    }
}
