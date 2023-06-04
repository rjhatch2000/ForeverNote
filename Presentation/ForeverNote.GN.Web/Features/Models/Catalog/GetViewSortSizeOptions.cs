using ForeverNote.Core.Domain.Localization;
using ForeverNote.Web.Models.Catalog;
using MediatR;

namespace ForeverNote.Web.Features.Models.Catalog
{
    public class GetViewSortSizeOptions : IRequest<(CatalogPagingFilteringModel pagingFilteringModel, CatalogPagingFilteringModel command)>
    {
        public Language Language { get; set; }
        public CatalogPagingFilteringModel PagingFilteringModel { get; set; }
        public CatalogPagingFilteringModel Command { get; set; }
        public bool AllowCustomersToSelectPageSize { get; set; }
        public string PageSizeOptions { get; set; }
        public int PageSize { get; set; }
    }
}
