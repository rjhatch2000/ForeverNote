using ForeverNote.Core;
using ForeverNote.Core.Caching;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Shipping;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Core.Plugins;
using ForeverNote.Core.Tests.Caching;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Common;
using ForeverNote.Services.Directory;
using ForeverNote.Services.Events;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Logging;
using ForeverNote.Services.Orders;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Shipping.Tests
{
    [TestClass()]
    public class ShipmentServiceTests
    {
        private IRepository<ShippingMethod> _shippingMethodRepository;
        private IRepository<DeliveryDate> _deliveryDateRepository;
        private IRepository<Warehouse> _warehouseRepository;
        private ILogger _logger;
        private IProductAttributeParser _productAttributeParser;
        private ICheckoutAttributeParser _checkoutAttributeParser;
        private ShippingSettings _shippingSettings;
        private IMediator _eventPublisher;
        private ILocalizationService _localizationService;
        private IAddressService _addressService;
        private ShoppingCartSettings _shoppingCartSettings;
        private IProductService _productService;
        private Store _store;
        private IStoreContext _storeContext;
        private IServiceProvider _serviceProvider;
        private ICountryService _countryService;
        private IStateProvinceService _stateProvinceService;
        private ICurrencyService _currencyService;

        [TestInitialize()]
        public void TestInitialize()
        {
            //plugin initialization
            new ForeverNote.Services.Tests.ServiceTest().PluginInitializator();

            _shippingSettings = new ShippingSettings();
            _shippingSettings.ActiveShippingRateComputationMethodSystemNames = new List<string>();
            _shippingSettings.ActiveShippingRateComputationMethodSystemNames.Add("FixedRateTestShippingRateComputationMethod");

            _shippingMethodRepository = new Mock<IRepository<ShippingMethod>>().Object;
            _deliveryDateRepository = new Mock<IRepository<DeliveryDate>>().Object;
            _warehouseRepository = new Mock<IRepository<Warehouse>>().Object;
            _logger = new NullLogger();
            _productAttributeParser = new Mock<IProductAttributeParser>().Object;
            _checkoutAttributeParser = new Mock<ICheckoutAttributeParser>().Object;
            _serviceProvider = new Mock<IServiceProvider>().Object;
            
            var tempEventPublisher = new Mock<IMediator>();
            {
                //tempEventPublisher.Setup(x => x.Publish(It.IsAny<object>()));
                _eventPublisher = tempEventPublisher.Object;
            }

            var cacheManager = new TestMemoryCacheManager(new Mock<IMemoryCache>().Object, _eventPublisher);

            var pluginFinder = new PluginFinder(_serviceProvider);
            _countryService = new Mock<ICountryService>().Object;
            _stateProvinceService = new Mock<IStateProvinceService>().Object;
            _currencyService = new Mock<ICurrencyService>().Object;
            _productService = new Mock<IProductService>().Object;

            

            _localizationService = new Mock<ILocalizationService>().Object;
            _addressService = new Mock<IAddressService>().Object;

            _store = new Store { Id = "1" };
            var tempStoreContext = new Mock<IStoreContext>();
            {
                tempStoreContext.Setup(x => x.CurrentStore).Returns(_store);
                _storeContext = tempStoreContext.Object;
            }

            _shoppingCartSettings = new ShoppingCartSettings();
        }

        //TO DO
        //[TestMethod()]
        //public void Can_load_shippingRateComputationMethods()
        //{
        //    var srcm = _shippingService.LoadAllShippingRateComputationMethods();
        //    Assert.IsNotNull(srcm);
        //    Assert.IsTrue(srcm.Count > 0);
        //}

        //[TestMethod()]
        //public void Can_load_shippingRateComputationMethod_by_systemKeyword()
        //{
        //    var srcm = _shippingService.LoadShippingRateComputationMethodBySystemName("FixedRateTestShippingRateComputationMethod");
        //    Assert.IsNotNull(srcm);
        //}

        //TO DO
        //[TestMethod()]
        //public async Task Can_load_active_shippingRateComputationMethods()
        //{
        //    var srcm = await _shippingService.LoadActiveShippingRateComputationMethods();
        //    Assert.IsNotNull(srcm);
        //    Assert.IsTrue(srcm.Count > 0);
        //}
    }
}