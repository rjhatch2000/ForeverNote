using ForeverNote.Api.DTOs.Common;
using ForeverNote.Api.Queries.Models.Common;
using ForeverNote.Core.Data;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Api.Queries.Handlers.Common
{
    public class GetStateProvinceQueryHandler : IRequestHandler<GetQuery<StateProvinceDto>, IMongoQueryable<StateProvinceDto>>
    {
        private readonly IMongoDBContext _mongoDBContext;

        public GetStateProvinceQueryHandler(IMongoDBContext mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }
        public Task<IMongoQueryable<StateProvinceDto>> Handle(GetQuery<StateProvinceDto> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id))
                return Task.FromResult(_mongoDBContext.Database().GetCollection<StateProvinceDto>(typeof(Core.Domain.Directory.StateProvince).Name).AsQueryable());
            else
                return Task.FromResult(_mongoDBContext.Database().GetCollection<StateProvinceDto>(typeof(Core.Domain.Directory.StateProvince).Name).AsQueryable().Where(x => x.Id == request.Id));
        }
    }
}
