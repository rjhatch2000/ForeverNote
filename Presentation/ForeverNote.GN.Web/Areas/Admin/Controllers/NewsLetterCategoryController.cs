using ForeverNote.Web.Framework.Controllers;
using ForeverNote.Web.Framework.Kendoui;
using ForeverNote.Web.Framework.Mvc.Filters;
using ForeverNote.Web.Framework.Security.Authorization;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Messages;
using ForeverNote.Services.Security;
using ForeverNote.Services.Stores;
using ForeverNote.Web.Areas.Admin.Extensions;
using ForeverNote.Web.Areas.Admin.Models.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.NewsletterSubscribers)]
    public partial class NewsletterCategoryController: BaseAdminController
    {
        #region Fields 

        private readonly INewsletterCategoryService _newsletterCategoryService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;

        #endregion

        #region Ctor

        public NewsletterCategoryController(INewsletterCategoryService newsletterCategoryService, ILanguageService languageService,
            ILocalizationService localizationService, IStoreService storeService)
        {
            this._newsletterCategoryService = newsletterCategoryService;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._storeService = storeService;
        }

        #endregion

        #region Methods

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var newslettercategories = await _newsletterCategoryService.GetAllNewsletterCategory();
            var gridModel = new DataSourceResult
            {
                Data = newslettercategories.Select(x =>
                {
                    return new {
                        Id = x.Id,
                        Name = x.Name,
                        Selected = x.Selected,
                        DisplayOrder = x.DisplayOrder
                    };
                }).OrderBy(x=>x.DisplayOrder),
                Total = newslettercategories.Count
            };
            return Json(gridModel);
        }

        public async Task<IActionResult> Create()
        {
            var model = new NewsletterCategoryModel();
            //locales
            await AddLocales(_languageService, model.Locales);
            //Stores
            await model.PrepareStoresMappingModel(null, _storeService, false);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(NewsletterCategoryModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var newsletterCategory = model.ToEntity();
                await _newsletterCategoryService.InsertNewsletterCategory(newsletterCategory);
                SuccessNotification(_localizationService.GetResource("Admin.Promotions.NewsletterCategory.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = newsletterCategory.Id }) : RedirectToAction("List");
            }

            //Stores
            await model.PrepareStoresMappingModel(null, _storeService, false);

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var newsletterCategory = await _newsletterCategoryService.GetNewsletterCategoryById(id);
            if (newsletterCategory == null)
                return RedirectToAction("List");

            var model = newsletterCategory.ToModel();

            //locales
            await AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = newsletterCategory.GetLocalized(x => x.Name, languageId, false, false);
                locale.Description = newsletterCategory.GetLocalized(x => x.Description, languageId, false, false);
            });

            //Stores
            await model.PrepareStoresMappingModel(newsletterCategory, _storeService, false);
            return View(model);
        }

        [HttpPost]
        [ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public async Task<IActionResult> Edit(NewsletterCategoryModel model, bool continueEditing)
        {
            var newsletterCategory = await _newsletterCategoryService.GetNewsletterCategoryById(model.Id);
            if (newsletterCategory == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                newsletterCategory = model.ToEntity(newsletterCategory);
                await _newsletterCategoryService.UpdateNewsletterCategory(newsletterCategory);

                SuccessNotification(_localizationService.GetResource("Admin.Promotions.NewsletterCategory.Updated"));
                return continueEditing ? RedirectToAction("Edit", new { id = newsletterCategory.Id }) : RedirectToAction("List");
            }
            //Stores
            await model.PrepareStoresMappingModel(newsletterCategory, _storeService, true);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var newsletterCategory = await _newsletterCategoryService.GetNewsletterCategoryById(id);
            if (newsletterCategory == null)
                return RedirectToAction("List");

            await _newsletterCategoryService.DeleteNewsletterCategory(newsletterCategory);

            SuccessNotification(_localizationService.GetResource("Admin.Promotions.NewsletterCategory.Deleted"));
            return RedirectToAction("List");
        }

        #endregion


    }
}