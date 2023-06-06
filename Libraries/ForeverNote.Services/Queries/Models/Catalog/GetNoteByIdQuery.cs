using ForeverNote.Core.Domain.Notes;
using MediatR;

namespace ForeverNote.Services.Queries.Models.Catalog
{
    public class GetNoteByIdQuery : IRequest<Note>
    {
        public string Id { get; set; }
    }
}
