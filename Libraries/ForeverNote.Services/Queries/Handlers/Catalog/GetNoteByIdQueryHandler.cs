using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Notes;
using ForeverNote.Services.Queries.Models.Catalog;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Queries.Handlers.Catalog
{
    public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, Note>
    {
        private readonly IRepository<Note> _noteRepository;

        public GetNoteByIdQueryHandler(IRepository<Note> noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public Task<Note> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Id))
                return Task.FromResult<Note>(null);

            return _noteRepository.GetByIdAsync(request.Id);
        }
    }
}
