﻿using ForeverNote.Core;
using ForeverNote.Core.Domain.Blogs;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Services.Blogs;
using ForeverNote.Services.Customers;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Logging;
using ForeverNote.Services.Messages;
using ForeverNote.Web.Commands.Models.Blogs;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Commands.Handler.Blogs
{
    public class InsertBlogCommentCommandHandler : IRequestHandler<InsertBlogCommentCommand, BlogComment>
    {
        private readonly IBlogService _blogService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ICustomerService _customerService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly ILocalizationService _localizationService;

        private readonly LocalizationSettings _localizationSettings;
        private readonly BlogSettings _blogSettings;

        public InsertBlogCommentCommandHandler(IBlogService blogService, IWorkContext workContext, IStoreContext storeContext,
            ICustomerService customerService, ICustomerActivityService customerActivityService, IWorkflowMessageService workflowMessageService,
            ILocalizationService localizationService, LocalizationSettings localizationSettings, BlogSettings blogSettings)
        {
            _blogService = blogService;
            _workContext = workContext;
            _storeContext = storeContext;
            _customerService = customerService;
            _customerActivityService = customerActivityService;
            _workflowMessageService = workflowMessageService;
            _localizationService = localizationService;

            _localizationSettings = localizationSettings;
            _blogSettings = blogSettings;
        }

        public async Task<BlogComment> Handle(InsertBlogCommentCommand request, CancellationToken cancellationToken)
        {
            var customer = _workContext.CurrentCustomer;
            var comment = new BlogComment {
                BlogPostId = request.BlogPost.Id,
                CustomerId = customer.Id,
                StoreId = _storeContext.CurrentStore.Id,
                CommentText = request.Model.AddNewComment.CommentText,
                CreatedOnUtc = DateTime.UtcNow,
                BlogPostTitle = request.BlogPost.Title,
            };
            await _blogService.InsertBlogComment(comment);

            //update totals
            var comments = await _blogService.GetBlogCommentsByBlogPostId(request.BlogPost.Id);
            request.BlogPost.CommentCount = comments.Count;
            await _blogService.UpdateBlogPost(request.BlogPost);
            if (!customer.HasContributions)
            {
                await _customerService.UpdateContributions(customer);
            }
            //notify a store owner
            if (_blogSettings.NotifyAboutNewBlogComments)
                await _workflowMessageService.SendBlogCommentNotificationMessage(request.BlogPost, comment, _localizationSettings.DefaultAdminLanguageId);

            //activity log
            await _customerActivityService.InsertActivity("PublicStore.AddBlogComment", comment.Id, _localizationService.GetResource("ActivityLog.PublicStore.AddBlogComment"));

            return comment;
        }
    }
}
