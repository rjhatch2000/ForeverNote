﻿using ForeverNote.Core.Caching;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Infrastructure.Cache
{
    public class ShoppingCartItemNotificatioHandler :
        INotificationHandler<EntityUpdated<ShoppingCartItem>>
    {

        private readonly ICacheManager _cacheManager;

        public ShoppingCartItemNotificatioHandler(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task Handle(EntityUpdated<ShoppingCartItem> eventMessage, CancellationToken cancellationToken)
        {
            await _cacheManager.RemoveByPrefix(string.Format(ModelCacheEventConst.CART_PICTURE_PATTERN_KEY, eventMessage.Entity.ProductId));
        }
    }
}