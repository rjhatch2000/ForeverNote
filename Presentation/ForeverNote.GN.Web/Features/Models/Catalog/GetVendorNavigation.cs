using ForeverNote.Core.Domain.Localization;
using ForeverNote.Web.Models.Catalog;
using MediatR;

namespace ForeverNote.Web.Features.Models.Catalog
{
    public class GetVendorNavigation : IRequest<VendorNavigationModel>
    {
        public Language Language { get; set; }
    }
}
