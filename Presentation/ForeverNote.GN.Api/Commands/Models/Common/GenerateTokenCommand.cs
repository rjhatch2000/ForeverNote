using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Api.Commands.Models.Common
{
    public class GenerateTokenCommand : IRequest<string>
    {
        public Dictionary<string, string> Claims { get; set; }
    }
}
