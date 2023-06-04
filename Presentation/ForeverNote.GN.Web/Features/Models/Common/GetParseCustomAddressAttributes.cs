using MediatR;
using Microsoft.AspNetCore.Http;

namespace ForeverNote.Web.Features.Models.Common
{
    public class GetParseCustomAddressAttributes : IRequest<string>
    {
        public IFormCollection Form { get; set; }
    }
}
