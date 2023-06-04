using ForeverNote.Core.Domain.Localization;
using ForeverNote.Web.Models.Catalog;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Web.Features.Models.Catalog
{
    public class GetVendorAll : IRequest<IList<VendorModel>>
    {
        public Language Language { get; set; }
    }
}
