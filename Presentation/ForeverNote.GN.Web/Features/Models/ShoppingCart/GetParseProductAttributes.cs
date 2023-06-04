using ForeverNote.Core.Domain.Catalog;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ForeverNote.Web.Features.Models.ShoppingCart
{
    public class GetParseProductAttributes : IRequest<string>
    {
        public Product Product { get; set; }
        public IFormCollection Form { get; set; }
    }
}
