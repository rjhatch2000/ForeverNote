using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Catalog;
using MediatR;

namespace ForeverNote.Web.Features.Models.Catalog
{
    public class GetProductTagsAll : IRequest<PopularProductTagsModel>
    {
        public Language Language { get; set; }
        public Store Store { get; set; }
    }
}
