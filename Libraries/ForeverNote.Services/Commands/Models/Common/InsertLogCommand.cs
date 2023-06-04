using ForeverNote.Core.Domain.Logging;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Common
{
    public class InsertLogCommand : IRequest<bool>
    {
        public LogLevel LogLevel { get; set; }
        public string ShortMessage { get; set; }
        public string FullMessage { get; set; }
    }
}
