using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Logging;
using ForeverNote.Web.Areas.Admin.Commands.Model.Common;
using MediatR;
using MongoDB.Bson;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Admin.Commands.Handler.Common
{
    public class DeleteActivitylogCommandHandler : IRequestHandler<DeleteActivitylogCommand, bool>
    {
        private readonly IRepository<ActivityLog> _repositoryActivityLog;

        public DeleteActivitylogCommandHandler(IRepository<ActivityLog> repositoryActivityLog)
        {
            _repositoryActivityLog = repositoryActivityLog;
        }

        public async Task<bool> Handle(DeleteActivitylogCommand request, CancellationToken cancellationToken)
        {
            await _repositoryActivityLog.Collection.DeleteManyAsync(new BsonDocument());
            return true;
        }
    }
}
