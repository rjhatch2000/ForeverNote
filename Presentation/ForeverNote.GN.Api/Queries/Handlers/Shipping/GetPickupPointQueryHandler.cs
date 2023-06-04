﻿using ForeverNote.Api.DTOs.Shipping;
using ForeverNote.Api.Queries.Models.Common;
using ForeverNote.Core.Data;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Api.Queries.Handlers.Shipping
{
    public class GetPickupPointQueryHandler : IRequestHandler<GetQuery<PickupPointDto>, IMongoQueryable<PickupPointDto>>
    {
        private readonly IMongoDBContext _mongoDBContext;

        public GetPickupPointQueryHandler(IMongoDBContext mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }
        public Task<IMongoQueryable<PickupPointDto>> Handle(GetQuery<PickupPointDto> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id))
                return Task.FromResult
                    (_mongoDBContext.Database()
                    .GetCollection<PickupPointDto>
                    (typeof(Core.Domain.Shipping.PickupPoint).Name)
                    .AsQueryable());
            else
                return Task.FromResult
                    (_mongoDBContext.Database()
                    .GetCollection<PickupPointDto>
                    (typeof(Core.Domain.Shipping.PickupPoint).Name)
                    .AsQueryable()
                    .Where(x => x.Id == request.Id));
        }
    }
}