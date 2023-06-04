﻿using ForeverNote.Core.Caching;
using ForeverNote.Core.Domain.Discounts;
using ForeverNote.Core.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Discounts.Cache
{
    /// <summary>
    /// Cache event consumer (used for caching of discount requirements)
    /// </summary>
    public abstract class DiscountRequirementEventConsumer :
        //discounts
        INotificationHandler<EntityUpdated<Discount>>,
        INotificationHandler<EntityDeleted<Discount>>

    {
        /// <summary>
        /// Key for discount requirement of a certain discount
        /// </summary>
        /// <remarks>
        /// {0} : discount id
        /// </remarks>
        public const string DISCOUNT_REQUIREMENT_MODEL_KEY = "ForeverNote.discountrequirements.all-{0}";
        public const string DISCOUNT_REQUIREMENT_PATTERN_KEY = "ForeverNote.discountrequirements";

        private readonly ICacheManager _cacheManager;

        public DiscountRequirementEventConsumer(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task Handle(EntityUpdated<Discount> notification, CancellationToken cancellationToken)
        {
            await _cacheManager.RemoveByPrefix(DISCOUNT_REQUIREMENT_PATTERN_KEY);
        }

        public async Task Handle(EntityDeleted<Discount> notification, CancellationToken cancellationToken)
        {
            await _cacheManager.RemoveByPrefix(DISCOUNT_REQUIREMENT_PATTERN_KEY);
        }

    }
}