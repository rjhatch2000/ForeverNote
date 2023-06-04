using MediatR;

namespace ForeverNote.Web.Features.Models.Products
{
    public class GetProductTemplateViewPath : IRequest<string>
    {
        public string ProductTemplateId { get; set; }
    }
}
