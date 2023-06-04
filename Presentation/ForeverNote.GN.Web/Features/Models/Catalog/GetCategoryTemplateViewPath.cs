using MediatR;

namespace ForeverNote.Web.Features.Models.Catalog
{
    public class GetCategoryTemplateViewPath : IRequest<string>
    {
        public string TemplateId { get; set; }
    }
}
