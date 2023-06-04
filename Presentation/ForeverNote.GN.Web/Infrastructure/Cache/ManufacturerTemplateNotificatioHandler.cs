using ForeverNote.Core.Caching;
using ForeverNote.Core.Domain.Blogs;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.News;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Polls;
using ForeverNote.Core.Domain.Topics;
using ForeverNote.Core.Domain.Vendors;
using ForeverNote.Core.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Infrastructure.Cache
{
    public partial class ManufacturerTemplateNotificatioHandler :
        INotificationHandler<EntityInserted<ManufacturerTemplate>>,
        INotificationHandler<EntityUpdated<ManufacturerTemplate>>,
        INotificationHandler<EntityDeleted<ManufacturerTemplate>>
    {
        private readonly ICacheManager _cacheManager;

        public ManufacturerTemplateNotificatioHandler(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task Handle(EntityInserted<ManufacturerTemplate> eventMessage, CancellationToken cancellationToken)
        {
            await _cacheManager.RemoveByPrefix(ModelCacheEventConst.MANUFACTURER_TEMPLATE_PATTERN_KEY);
        }
        public async Task Handle(EntityUpdated<ManufacturerTemplate> eventMessage, CancellationToken cancellationToken)
        {
            await _cacheManager.RemoveByPrefix(ModelCacheEventConst.MANUFACTURER_TEMPLATE_PATTERN_KEY);
        }
        public async Task Handle(EntityDeleted<ManufacturerTemplate> eventMessage, CancellationToken cancellationToken)
        {
            await _cacheManager.RemoveByPrefix(ModelCacheEventConst.MANUFACTURER_TEMPLATE_PATTERN_KEY);
        }

    }
}
