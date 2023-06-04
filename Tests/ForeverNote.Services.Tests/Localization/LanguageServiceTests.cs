﻿using ForeverNote.Core.Caching;
using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Tests.Caching;
using ForeverNote.Services.Configuration;
using ForeverNote.Services.Events;
using ForeverNote.Services.Stores;
using ForeverNote.Services.Tests;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace ForeverNote.Services.Localization.Tests
{
    [TestClass()]
    public class LanguageServiceTests
    {
        private IRepository<Language> _languageRepo;
        private IStoreMappingService _storeMappingService;
        private ILanguageService _languageService;
        private ISettingService _settingService;
        private IMediator _eventPublisher;
        private LocalizationSettings _localizationSettings;

        [TestInitialize()]
        public void TestInitialize()
        {
            var lang1 = new Language
            {
                Name = "English",
                LanguageCulture = "en-Us",
                FlagImageFileName = "us.png",
                Published = true,
                DisplayOrder = 1
            };
            var lang2 = new Language
            {
                Name = "Russian",
                LanguageCulture = "ru-Ru",
                FlagImageFileName = "ru.png",
                Published = true,
                DisplayOrder = 2
            };

            _languageRepo = new MongoDBRepositoryTest<Language>();
            _languageRepo.Insert(lang1);
            _languageRepo.Insert(lang2);

            _storeMappingService = new Mock<IStoreMappingService>().Object;
            _settingService = new Mock<ISettingService>().Object;
            var tempEventPublisher = new Mock<IMediator>();
            {
                //tempEventPublisher.Setup(x => x.PublishAsync(It.IsAny<object>()));
                _eventPublisher = tempEventPublisher.Object;
            }
            _localizationSettings = new LocalizationSettings();

            _languageService = new LanguageService(new TestMemoryCacheManager(new Mock<IMemoryCache>().Object, _eventPublisher), _languageRepo, 
                _settingService, _localizationSettings, _eventPublisher);
        }

        [TestMethod()]
        public async Task Can_get_all_languages()
        {
            var languages = await _languageService.GetAllLanguages();
            Assert.IsNotNull(languages);
            Assert.IsTrue(languages.Count > 0);
        }
    }
}