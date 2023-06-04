using ForeverNote.Core;
using ForeverNote.Core.Caching;
using ForeverNote.Core.Domain.Blogs;
using ForeverNote.Core.Domain.Media;
using ForeverNote.Services.Blogs;
using ForeverNote.Services.Helpers;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Media;
using ForeverNote.Services.Seo;
using ForeverNote.Web.Features.Models.Blogs;
using ForeverNote.Web.Infrastructure.Cache;
using ForeverNote.Web.Models.Blogs;
using ForeverNote.Web.Models.Media;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Features.Handlers.Blogs
{
    public class GetHomePageBlogHandler : IRequestHandler<GetHomePageBlog, HomePageBlogItemsModel>
    {
        private readonly IBlogService _blogService;
        private readonly ICacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IWebHelper _webHelper;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IPictureService _pictureService;
        private readonly ILocalizationService _localizationService;

        private readonly BlogSettings _blogSettings;
        private readonly MediaSettings _mediaSettings;

        public GetHomePageBlogHandler(IBlogService blogService,
            IWorkContext workContext,
            IStoreContext storeContext,
            IPictureService pictureService,
            ILocalizationService localizationService,
            IDateTimeHelper dateTimeHelper,
            IWebHelper webHelper,
            ICacheManager cacheManager,
            BlogSettings blogSettings,
            MediaSettings mediaSettings)
        {
            _blogService = blogService;
            _workContext = workContext;
            _storeContext = storeContext;
            _pictureService = pictureService;
            _localizationService = localizationService;
            _dateTimeHelper = dateTimeHelper;
            _webHelper = webHelper;
            _cacheManager = cacheManager;

            _blogSettings = blogSettings;
            _mediaSettings = mediaSettings;
        }

        public async Task<HomePageBlogItemsModel> Handle(GetHomePageBlog request, CancellationToken cancellationToken)
        {
            var cacheKey = string.Format(ModelCacheEventConst.BLOG_HOMEPAGE_MODEL_KEY, _workContext.WorkingLanguage.Id, 
                _storeContext.CurrentStore.Id, _webHelper.GetMachineName());
            var cachedModel = await _cacheManager.GetAsync(cacheKey, async () =>
            {
                var model = new HomePageBlogItemsModel();

                var blogPosts = await _blogService.GetAllBlogPosts(_storeContext.CurrentStore.Id,
                        null, null, 0, _blogSettings.HomePageBlogCount);

                foreach (var post in blogPosts)
                {
                    var item = new HomePageBlogItemsModel.BlogItemModel();
                    var description = post.GetLocalized(x => x.BodyOverview, _workContext.WorkingLanguage.Id);
                    item.SeName = post.GetSeName(_workContext.WorkingLanguage.Id);
                    item.Title = post.GetLocalized(x => x.Title, _workContext.WorkingLanguage.Id);
                    item.Short = description?.Length > _blogSettings.MaxTextSizeHomePage ? description.Substring(0, _blogSettings.MaxTextSizeHomePage) : description;
                    item.CreatedOn = _dateTimeHelper.ConvertToUserTime(post.StartDateUtc ?? post.CreatedOnUtc, DateTimeKind.Utc);
                    item.GenericAttributes = post.GenericAttributes;

                    //prepare picture model
                    if (!string.IsNullOrEmpty(post.PictureId))
                    {
                        var pictureModel = new PictureModel {
                            Id = post.PictureId,
                            FullSizeImageUrl = await _pictureService.GetPictureUrl(post.PictureId),
                            ImageUrl = await _pictureService.GetPictureUrl(post.PictureId, _mediaSettings.BlogThumbPictureSize),
                            Title = string.Format(_localizationService.GetResource("Media.Blog.ImageLinkTitleFormat"), post.Title),
                            AlternateText = string.Format(_localizationService.GetResource("Media.Blog.ImageAlternateTextFormat"), post.Title)
                        };
                        item.PictureModel = pictureModel;
                    }
                    model.Items.Add(item);
                }
                return model;
            });

            return cachedModel;
        }
    }
}
