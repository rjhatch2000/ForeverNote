using ForeverNote.Core;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.News;
using ForeverNote.Services.Customers;
using ForeverNote.Services.Messages;
using ForeverNote.Services.News;
using ForeverNote.Web.Commands.Models.News;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Commands.Handler.News
{
    public class InsertNewsCommentCommandHandler : IRequestHandler<InsertNewsCommentCommand, NewsComment>
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly INewsService _newsService;
        private readonly ICustomerService _customerService;
        private readonly IWorkflowMessageService _workflowMessageService;

        private readonly NewsSettings _newsSettings;
        private readonly LocalizationSettings _localizationSettings;

        public InsertNewsCommentCommandHandler(IWorkContext workContext, IStoreContext storeContext, INewsService newsService,
            ICustomerService customerService, IWorkflowMessageService workflowMessageService, NewsSettings newsSettings,
            LocalizationSettings localizationSettings)
        {
            _workContext = workContext;
            _storeContext = storeContext;
            _newsService = newsService;
            _customerService = customerService;
            _workflowMessageService = workflowMessageService;

            _newsSettings = newsSettings;
            _localizationSettings = localizationSettings;
        }

        public async Task<NewsComment> Handle(InsertNewsCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new NewsComment {
                NewsItemId = request.NewsItem.Id,
                CustomerId = _workContext.CurrentCustomer.Id,
                StoreId = _storeContext.CurrentStore.Id,
                CommentTitle = request.Model.AddNewComment.CommentTitle,
                CommentText = request.Model.AddNewComment.CommentText,
                CreatedOnUtc = DateTime.UtcNow,
            };
            request.NewsItem.NewsComments.Add(comment);

            //update totals
            request.NewsItem.CommentCount = request.NewsItem.NewsComments.Count;

            await _newsService.UpdateNews(request.NewsItem);

            await _customerService.UpdateContributions(_workContext.CurrentCustomer);

            //notify a store owner;
            if (_newsSettings.NotifyAboutNewNewsComments)
                await _workflowMessageService.SendNewsCommentNotificationMessage(request.NewsItem, comment, _localizationSettings.DefaultAdminLanguageId);

            return comment;
        }
    }
}
