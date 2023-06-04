using ForeverNote.Core;
using ForeverNote.Core.Domain.Blogs;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Media;
using ForeverNote.Web.Framework.Security.Captcha;
using ForeverNote.Services.Blogs;
using ForeverNote.Services.Common;
using ForeverNote.Services.Customers;
using ForeverNote.Services.Helpers;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Media;
using ForeverNote.Services.Seo;
using ForeverNote.Web.Features.Models.Blogs;
using ForeverNote.Web.Models.Blogs;
using ForeverNote.Web.Models.Media;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Features.Handlers.Blogs
{
    public class GetBlogPostHandler : IRequestHandler<GetBlogPost, BlogPostModel>
    {
        private readonly IBlogService _blogService;
        private readonly IWorkContext _workContext;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IPictureService _pictureService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerService _customerService;

        private readonly MediaSettings _mediaSettings;
        private readonly CaptchaSettings _captchaSettings;
        private readonly CustomerSettings _customerSettings;

        public GetBlogPostHandler(
           IBlogService blogService,
           IWorkContext workContext,
           IPictureService pictureService,
           ILocalizationService localizationService,
           IDateTimeHelper dateTimeHelper,
           ICustomerService customerService,
           CaptchaSettings captchaSettings,
           MediaSettings mediaSettings,
           CustomerSettings customerSettings)
        {
            _blogService = blogService;
            _workContext = workContext;
            _pictureService = pictureService;
            _localizationService = localizationService;
            _dateTimeHelper = dateTimeHelper;
            _customerService = customerService;

            _captchaSettings = captchaSettings;
            _mediaSettings = mediaSettings;
            _customerSettings = customerSettings;
        }

        public async Task<BlogPostModel> Handle(GetBlogPost request, CancellationToken cancellationToken)
        {
            if (request.BlogPost == null)
                throw new ArgumentNullException("blogPost");

            var model = new BlogPostModel();

            model.Id = request.BlogPost.Id;
            model.MetaTitle = request.BlogPost.GetLocalized(x => x.MetaTitle, _workContext.WorkingLanguage.Id);
            model.MetaDescription = request.BlogPost.GetLocalized(x => x.MetaDescription, _workContext.WorkingLanguage.Id);
            model.MetaKeywords = request.BlogPost.GetLocalized(x => x.MetaKeywords, _workContext.WorkingLanguage.Id);
            model.SeName = request.BlogPost.GetSeName(_workContext.WorkingLanguage.Id);
            model.Title = request.BlogPost.GetLocalized(x => x.Title, _workContext.WorkingLanguage.Id);
            model.Body = request.BlogPost.GetLocalized(x => x.Body, _workContext.WorkingLanguage.Id);
            model.BodyOverview = request.BlogPost.GetLocalized(x => x.BodyOverview, _workContext.WorkingLanguage.Id);
            model.AllowComments = request.BlogPost.AllowComments;
            model.CreatedOn = _dateTimeHelper.ConvertToUserTime(request.BlogPost.StartDateUtc ?? request.BlogPost.CreatedOnUtc, DateTimeKind.Utc);
            model.Tags = request.BlogPost.ParseTags().ToList();
            model.NumberOfComments = request.BlogPost.CommentCount;
            model.AddNewComment.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnBlogCommentPage;
            model.GenericAttributes = request.BlogPost.GenericAttributes;

            var blogComments = await _blogService.GetBlogCommentsByBlogPostId(request.BlogPost.Id);
            foreach (var bc in blogComments)
            {
                var commentModel = await PrepareBlogPostCommentModel(bc);
                model.Comments.Add(commentModel);
            }

            //prepare picture model
            await PrepareBlogPostPictureModel(model, request.BlogPost);

            return model;
        }
        private async Task PrepareBlogPostPictureModel(BlogPostModel model, BlogPost blogPost)
        {
            if (!string.IsNullOrEmpty(blogPost.PictureId))
            {
                var pictureModel = new PictureModel {
                    Id = blogPost.PictureId,
                    FullSizeImageUrl = await _pictureService.GetPictureUrl(blogPost.PictureId),
                    ImageUrl = await _pictureService.GetPictureUrl(blogPost.PictureId, _mediaSettings.BlogThumbPictureSize),
                    Title = string.Format(_localizationService.GetResource("Media.Blog.ImageLinkTitleFormat"), blogPost.Title),
                    AlternateText = string.Format(_localizationService.GetResource("Media.Blog.ImageAlternateTextFormat"), blogPost.Title)
                };

                model.PictureModel = pictureModel;
            }
        }

        private async Task<BlogCommentModel> PrepareBlogPostCommentModel(BlogComment blogComment)
        {
            var customer = await _customerService.GetCustomerById(blogComment.CustomerId);
            var model = new BlogCommentModel {
                Id = blogComment.Id,
                CustomerId = blogComment.CustomerId,
                CustomerName = customer.FormatUserName(_customerSettings.CustomerNameFormat),
                CommentText = blogComment.CommentText,
                CreatedOn = _dateTimeHelper.ConvertToUserTime(blogComment.CreatedOnUtc, DateTimeKind.Utc),
                AllowViewingProfiles = _customerSettings.AllowViewingProfiles && customer != null && !customer.IsGuest(),
            };
            if (_customerSettings.AllowCustomersToUploadAvatars)
            {
                model.CustomerAvatarUrl = await _pictureService.GetPictureUrl(
                    customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.AvatarPictureId),
                    _mediaSettings.AvatarPictureSize,
                    _customerSettings.DefaultAvatarEnabled,
                    defaultPictureType: PictureType.Avatar);
            }

            return model;
        }
    }
}
