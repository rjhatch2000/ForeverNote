using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Directory;
using MediatR;
namespace ForeverNote.Web.Features.Models.Catalog
{
    public class GetFormatBasePrice : IRequest<string>
    {
        public Currency Currency { get; set; }
        public Product Product { get; set; }
        public decimal? ProductPrice { get; set; }
    }
}
