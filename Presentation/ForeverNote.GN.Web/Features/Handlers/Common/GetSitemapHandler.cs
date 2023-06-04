using ForeverNote.Core.Caching;
using ForeverNote.Core.Domain.Blogs;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Forums;
using ForeverNote.Core.Domain.Knowledgebase;
using ForeverNote.Core.Domain.News;
using ForeverNote.Services.Blogs;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Customers;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Seo;
using ForeverNote.Services.Topics;
using ForeverNote.Web.Extensions;
using ForeverNote.Web.Features.Models.Common;
using ForeverNote.Web.Infrastructure.Cache;
using ForeverNote.Web.Models.Blogs;
using ForeverNote.Web.Models.Catalog;
using ForeverNote.Web.Models.Common;
using ForeverNote.Web.Models.Topics;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Features.Handlers.Common
{
    public class GetSitemapHandler : IRequestHandler<GetSitemap, SitemapModel>
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IProductService _productService;
        private readonly ITopicService _topicService;
        private readonly IBlogService _blogService;

        private readonly CommonSettings _commonSettings;
        private readonly BlogSettings _blogSettings;
        private readonly ForumSettings _forumSettings;
        private readonly NewsSettings _newsSettings;
        private readonly KnowledgebaseSettings _knowledgebaseSettings;

        public GetSitemapHandler(ICacheManager cacheManager,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IProductService productService,
            ITopicService topicService,
            IBlogService blogService,
            CommonSettings commonSettings,
            BlogSettings blogSettings,
            ForumSettings forumSettings,
            NewsSettings newsSettings,
            KnowledgebaseSettings knowledgebaseSettings)
        {
            _cacheManager = cacheManager;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _productService = productService;
            _topicService = topicService;
            _blogService = blogService;

            _commonSettings = commonSettings;
            _blogSettings = blogSettings;
            _forumSettings = forumSettings;
            _newsSettings = newsSettings;
            _knowledgebaseSettings = knowledgebaseSettings;
        }

        public async Task<SitemapModel> Handle(GetSitemap request, CancellationToken cancellationToken)
        {
            string cacheKey = string.Format(ModelCacheEventConst.SITEMAP_PAGE_MODEL_KEY,
                request.Language.Id,
                string.Join(",", request.Customer.GetCustomerRoleIds()),
                request.Store.Id);
            var cachedModel = await _cacheManager.GetAsync(cacheKey, async () =>
            {
                var model = new SitemapModel {
                    BlogEnabled = _blogSettings.Enabled,
                    ForumEnabled = _forumSettings.ForumsEnabled,
                    NewsEnabled = _newsSettings.Enabled,
                    KnowledgebaseEnabled = _knowledgebaseSettings.Enabled
                };
                //categories
                if (_commonSettings.SitemapIncludeCategories)
                {
                    var categories = await _categoryService.GetAllCategories();
                    model.Categories = categories.Select(x => x.ToModel(request.Language)).ToList();
                }
                //manufacturers
                if (_commonSettings.SitemapIncludeManufacturers)
                {
                    var manufacturers = await _manufacturerService.GetAllManufacturers();
                    model.Manufacturers = manufacturers.Select(x => x.ToModel(request.Language)).ToList();
                }
                //products
                if (_commonSettings.SitemapIncludeProducts)
                {
                    //limit product to 200 until paging is supported on this page
                    var products = (await _productService.SearchProducts(
                        storeId: request.Store.Id,
                        visibleIndividuallyOnly: true,
                        pageSize: 200)).products;
                    model.Products = products.Select(product => new ProductOverviewModel {
                        Id = product.Id,
                        Name = product.GetLocalized(x => x.Name, request.Language.Id),
                        ShortDescription = product.GetLocalized(x => x.ShortDescription, request.Language.Id),
                        FullDescription = product.GetLocalized(x => x.FullDescription, request.Language.Id),
                        SeName = product.GetSeName(request.Language.Id),
                    }).ToList();
                }

                //topics
                var topics = (await _topicService.GetAllTopics(request.Store.Id))
                    .Where(t => t.IncludeInSitemap)
                    .ToList();
                model.Topics = topics.Select(topic => new TopicModel {
                    Id = topic.Id,
                    SystemName = topic.SystemName,
                    IncludeInSitemap = topic.IncludeInSitemap,
                    IsPasswordProtected = topic.IsPasswordProtected,
                    Title = topic.GetLocalized(x => x.Title, request.Language.Id),
                }).ToList();

                //blog posts
                var blogposts = (await _blogService.GetAllBlogPosts(request.Store.Id))
                    .ToList();
                model.BlogPosts = blogposts.Select(blogpost => new BlogPostModel {
                    Id = blogpost.Id,
                    SeName = blogpost.GetSeName(request.Language.Id),
                    Title = blogpost.GetLocalized(x => x.Title, request.Language.Id),
                }).ToList();
                return model;
            });
            return cachedModel;
        }
    }
}
