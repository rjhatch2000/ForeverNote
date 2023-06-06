using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Notes;
using ForeverNote.Services.Commands.Models.Catalog;
using MediatR;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Commands.Handlers.Catalog
{
    public class DeleteMeasureUnitOnNoteCommandHandler : IRequestHandler<DeleteMeasureUnitOnNoteCommand, bool>
    {
        private readonly IRepository<Note> _repositoryNote;

        public DeleteMeasureUnitOnNoteCommandHandler(IRepository<Note> repositoryNote)
        {
            _repositoryNote = repositoryNote;
        }

        public async Task<bool> Handle(DeleteMeasureUnitOnNoteCommand request, CancellationToken cancellationToken)
        {
            var builder = Builders<Note>.Filter;
            var filter = builder.Eq(x => x.UnitId, request.MeasureUnitId);
            var update = Builders<Note>.Update
                .Set(x => x.UnitId, "");

            await _repositoryNote.Collection.UpdateManyAsync(filter, update);
            return true;
        }
    }
}
