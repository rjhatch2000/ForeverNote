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
    public class GetDeliveryDateQueryHandler : IRequestHandler<GetQuery<DeliveryDateDto>, IMongoQueryable<DeliveryDateDto>>
    {
        private readonly IMongoDBContext _mongoDBContext;

        public GetDeliveryDateQueryHandler(IMongoDBContext mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }
        public Task<IMongoQueryable<DeliveryDateDto>> Handle(GetQuery<DeliveryDateDto> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Id))
                return Task.FromResult
                    (_mongoDBContext.Database()
                    .GetCollection<DeliveryDateDto>
                    (typeof(Core.Domain.Shipping.DeliveryDate).Name)
                    .AsQueryable());
            else
                return Task.FromResult
                    (_mongoDBContext.Database()
                    .GetCollection<DeliveryDateDto>
                    (typeof(Core.Domain.Shipping.DeliveryDate).Name)
                    .AsQueryable()
                    .Where(x => x.Id == request.Id));
        }
    }
}