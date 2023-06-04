using System.Threading;
using System.Threading.Tasks;
using ForeverNote.Core.Domain.Blogs;
using ForeverNote.Core.Events;
using ForeverNote.Services.Seo;
using MediatR;

namespace ForeverNote.Services.Events
{
    public class BlogPostDeletedEventHandler : INotificationHandler<EntityDeleted<BlogPost>>
    {
        private readonly IUrlRecordService _urlRecordService;
        
        public BlogPostDeletedEventHandler(IUrlRecordService urlRecordService)
        {
            _urlRecordService = urlRecordService;
        }
        public async Task Handle(EntityDeleted<BlogPost> notification, CancellationToken cancellationToken)
        {
            var urlToDelete = await _urlRecordService.GetBySlug(notification.Entity.SeName);
            await _urlRecordService.DeleteUrlRecord(urlToDelete);
        }
    }
}
