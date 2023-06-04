using ForeverNote.Core;
using ForeverNote.Core.Caching;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Discounts;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Core.Domain.Vendors;
using ForeverNote.Core.Plugins;
using ForeverNote.Core.Tests.Caching;
using ForeverNote.Services.Common;
using ForeverNote.Services.Events;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Tests;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace ForeverNote.Services.Discounts.Tests
{
    [TestClass()] 
    public class DiscountServiceTests {
        private IRepository<Discount> _discountRepo;
        private IRepository<DiscountCoupon> _discountCouponRepo;
        private IRepository<DiscountUsageHistory> _discountUsageHistoryRepo;
        private IMediator _eventPublisher;
        private IGenericAttributeService _genericAttributeService;
        private ILocalizationService _localizationService;
        private IDiscountService _discountService;
        private IStoreContext _storeContext;
        private IServiceProvider _serviceProvider;

        private ShoppingCartSettings _shoppingCartSettings;
        private CatalogSettings _catalogSettings;

        [TestInitialize()]
        public void TestInitialize() {
            //plugin initialization
            new ForeverNote.Services.Tests.ServiceTest().PluginInitializator();

            var discount1 = new Discount {                
                DiscountType = DiscountType.AssignedToCategories,
                Name = "Discount 1",
                UsePercentage = true,       //!     (everything that I marked as "!" is variable that changes test output)
                DiscountPercentage = 10,    //!
                DiscountAmount = 0,
                DiscountLimitation = DiscountLimitationType.Unlimited,
                LimitationTimes = 0,
                IsEnabled = true,
            };
            var discount2 = new Discount {
                DiscountType = DiscountType.AssignedToSkus,
                Name = "Discount 2",
                UsePercentage = false,      //!
                DiscountPercentage = 0,     //!
                DiscountAmount = 5,         //use amount instad of percentage (e.g. discount 100PLN instead of discount 10%) 
                RequiresCouponCode = true,
                DiscountLimitation = DiscountLimitationType.NTimesPerCustomer, //only N times for this customer
                LimitationTimes = 3, // 3 times for this customer
                IsEnabled = true,
            };
            _discountRepo = new MongoDBRepositoryTest<Discount>();
            _discountRepo.Insert(discount1);
            _discountRepo.Insert(discount2);


            var tempEventPublisher = new Mock<IMediator>();
            {
                //tempEventPublisher.Setup(x => x.PublishAsync(It.IsAny<object>()));
                _eventPublisher = tempEventPublisher.Object;
            }
            _storeContext = new Mock<IStoreContext>().Object;
            _serviceProvider = new Mock<IServiceProvider>().Object;

            _discountUsageHistoryRepo = new Mock<IRepository<DiscountUsageHistory>>().Object;
            _discountCouponRepo = new Mock<IRepository<DiscountCoupon>>().Object;
            var extraProductRepo = new Mock<IRepository<Product>>().Object;
            var extraCategoryRepo = new Mock<IRepository<Category>>().Object;
            var extraManufacturerRepo = new Mock<IRepository<Manufacturer>>().Object;
            var extraStoreRepo = new Mock<IRepository<Store>>().Object;
            var extraVendorRepo = new Mock<IRepository<Vendor>>().Object;

            _genericAttributeService = new Mock<IGenericAttributeService>().Object;
            _localizationService = new Mock<ILocalizationService>().Object;
            _shoppingCartSettings = new Mock<ShoppingCartSettings>().Object;
            _catalogSettings = new Mock<CatalogSettings>().Object;

            _discountService = new DiscountService(new TestMemoryCacheManager(new Mock<IMemoryCache>().Object, _eventPublisher), _discountRepo, _discountCouponRepo,
                _discountUsageHistoryRepo, _localizationService, _storeContext,
                new PluginFinder(_serviceProvider), _eventPublisher, extraProductRepo, extraCategoryRepo, extraManufacturerRepo, extraVendorRepo, new PerRequestCacheManager(null),
                _shoppingCartSettings, _catalogSettings);
        }

        [TestMethod()]
        public async Task Can_get_all_discount() {
            var discounts = await _discountService.GetAllDiscounts(null);
            Assert.IsNotNull(discounts);
            Assert.AreEqual(2, discounts.Count);
        }

    }
}