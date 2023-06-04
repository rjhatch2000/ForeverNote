using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Directory;
using ForeverNote.Web.Framework.Kendoui;
using ForeverNote.Web.Framework.Mvc;
using ForeverNote.Web.Framework.Mvc.Filters;
using ForeverNote.Web.Framework.Security.Authorization;
using ForeverNote.Services.Customers;
using ForeverNote.Services.Directory;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Logging;
using ForeverNote.Services.Orders;
using ForeverNote.Services.Security;
using ForeverNote.Services.Stores;
using ForeverNote.Web.Areas.Admin.Extensions;
using ForeverNote.Web.Areas.Admin.Models.Orders;
using ForeverNote.Web.Areas.Admin.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.Attributes)]
    public partial class CheckoutAttributeController : BaseAdminController
    {
        #region Fields

        private readonly ICheckoutAttributeService _checkoutAttributeService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly ICurrencyService _currencyService;
        private readonly CurrencySettings _currencySettings;
        private readonly IMeasureService _measureService;
        private readonly MeasureSettings _measureSettings;
        private readonly IStoreService _storeService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly ICustomerService _customerService;
        private readonly ICheckoutAttributeViewModelService _checkoutAttributeViewModelService;

        #endregion

        #region Constructors

        public CheckoutAttributeController(ICheckoutAttributeService checkoutAttributeService,
            ILanguageService languageService, 
            ILocalizationService localizationService,
            ICurrencyService currencyService, 
            CurrencySettings currencySettings,
            IMeasureService measureService, 
            MeasureSettings measureSettings,
            IStoreService storeService,
            IStoreMappingService storeMappingService,
            ICustomerService customerService,
            ICheckoutAttributeViewModelService checkoutAttributeViewModelService)
        {
            this._checkoutAttributeService = checkoutAttributeService;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._currencyService = currencyService;
            this._currencySettings = currencySettings;
            this._measureService = measureService;
            this._measureSettings = measureSettings;
            this._storeService = storeService;
            this._storeMappingService = storeMappingService;
            this._customerService = customerService;
            this._checkoutAttributeViewModelService = checkoutAttributeViewModelService;
        }

        #endregion
        
        #region Checkout attributes

        //list
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var checkoutAttributes = await _checkoutAttributeViewModelService.PrepareCheckoutAttributeListModel();
            var gridModel = new DataSourceResult
            {
                Data = checkoutAttributes.ToList(),
                Total = checkoutAttributes.Count()
            };
            return Json(gridModel);
        }
        
        //create
        public async Task<IActionResult> Create()
        {
            var model = await _checkoutAttributeViewModelService.PrepareCheckoutAttributeModel();
            //locales
            await AddLocales(_languageService, model.Locales);
            //Stores
            await model.PrepareStoresMappingModel(null, _storeService, false);
            //ACL
            await model.PrepareACLModel(null, false, _customerService);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(CheckoutAttributeModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var checkoutAttribute = await _checkoutAttributeViewModelService.InsertCheckoutAttributeModel(model);
                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Attributes.CheckoutAttributes.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = checkoutAttribute.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            //tax categories
            await _checkoutAttributeViewModelService.PrepareTaxCategories(model, null, true);
            //Stores
            await model.PrepareStoresMappingModel(null, _storeService, true);
            //ACL
            await model.PrepareACLModel(null, true, _customerService);

            return View(model);
        }

        //edit
        public async Task<IActionResult> Edit(string id)
        {
            var checkoutAttribute = await _checkoutAttributeService.GetCheckoutAttributeById(id);
            if (checkoutAttribute == null)
                //No checkout attribute found with the specified id
                return RedirectToAction("List");

            var model = checkoutAttribute.ToModel();
            //locales
            await AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = checkoutAttribute.GetLocalized(x => x.Name, languageId, false, false);
                locale.TextPrompt = checkoutAttribute.GetLocalized(x => x.TextPrompt, languageId, false, false);
            });

            //tax categories
            await _checkoutAttributeViewModelService.PrepareTaxCategories(model, checkoutAttribute, false);
            //Stores
            await model.PrepareStoresMappingModel(checkoutAttribute, _storeService, false);

            //condition
            await _checkoutAttributeViewModelService.PrepareConditionAttributes(model, checkoutAttribute);
            //ACL
            await model.PrepareACLModel(checkoutAttribute, false, _customerService);
            //Stores
            await model.PrepareStoresMappingModel(checkoutAttribute, _storeService, false);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(CheckoutAttributeModel model, bool continueEditing)
        {
            var checkoutAttribute = await _checkoutAttributeService.GetCheckoutAttributeById(model.Id);
            if (checkoutAttribute == null)
                //No checkout attribute found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                checkoutAttribute = await _checkoutAttributeViewModelService.UpdateCheckoutAttributeModel(checkoutAttribute, model);
                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Attributes.CheckoutAttributes.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new {id = checkoutAttribute.Id});
                }
                return RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form

            //tax categories
            await _checkoutAttributeViewModelService.PrepareTaxCategories(model, checkoutAttribute, true);
            //Stores
            await model.PrepareStoresMappingModel(checkoutAttribute, _storeService, true);
            //ACL
            await model.PrepareACLModel(checkoutAttribute, true, _customerService);

            return View(model);
        }

        //delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id, [FromServices] ICustomerActivityService customerActivityService)
        {
            var checkoutAttribute = await _checkoutAttributeService.GetCheckoutAttributeById(id);
            await _checkoutAttributeService.DeleteCheckoutAttribute(checkoutAttribute);

            //activity log
            await customerActivityService.InsertActivity("DeleteCheckoutAttribute", checkoutAttribute.Id, _localizationService.GetResource("ActivityLog.DeleteCheckoutAttribute"), checkoutAttribute.Name);

            SuccessNotification(_localizationService.GetResource("Admin.Catalog.Attributes.CheckoutAttributes.Deleted"));
            return RedirectToAction("List");
        }

        #endregion

        #region Checkout attribute values

        //list
        [HttpPost]
        public async Task<IActionResult> ValueList(string checkoutAttributeId, DataSourceRequest command)
        {
            var checkoutAttribute = await _checkoutAttributeViewModelService.PrepareCheckoutAttributeValuesModel(checkoutAttributeId);
            var gridModel = new DataSourceResult
            {
                Data = checkoutAttribute.ToList(),
                Total = checkoutAttribute.Count()
            };
            return Json(gridModel);
        }

        //create
        public async Task<IActionResult> ValueCreatePopup(string checkoutAttributeId)
        {
            var model = await _checkoutAttributeViewModelService.PrepareCheckoutAttributeValueModel(checkoutAttributeId);
            //locales
            await AddLocales(_languageService, model.Locales);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ValueCreatePopup(CheckoutAttributeValueModel model)
        {
            var checkoutAttribute = await _checkoutAttributeService.GetCheckoutAttributeById(model.CheckoutAttributeId);
            if (checkoutAttribute == null)
                //No checkout attribute found with the specified id
                return RedirectToAction("List");

            model.PrimaryStoreCurrencyCode = (await _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId)).CurrencyCode;
            model.BaseWeightIn = (await _measureService.GetMeasureWeightById(_measureSettings.BaseWeightId)).Name;

            if (checkoutAttribute.AttributeControlType == AttributeControlType.ColorSquares)
            {
                //ensure valid color is chosen/entered
                if (String.IsNullOrEmpty(model.ColorSquaresRgb))
                    ModelState.AddModelError("", "Color is required");                
            }

            if (ModelState.IsValid)
            {
                var cav = await _checkoutAttributeViewModelService.InsertCheckoutAttributeValueModel(checkoutAttribute, model);
                ViewBag.RefreshPage = true;
                return View(model);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        //edit
        public async Task<IActionResult> ValueEditPopup(string id, string checkoutAttributeId)
        {
            var checkoutAttribute = await _checkoutAttributeService.GetCheckoutAttributeById(checkoutAttributeId);
            var cav = checkoutAttribute.CheckoutAttributeValues.Where(x=>x.Id == id).FirstOrDefault();
            if (cav == null)
                //No checkout attribute value found with the specified id
                return RedirectToAction("List");

            var model = await _checkoutAttributeViewModelService.PrepareCheckoutAttributeValueModel(checkoutAttribute, cav);

            //locales
            await AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = cav.GetLocalized(x => x.Name, languageId, false, false);
            });

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ValueEditPopup(CheckoutAttributeValueModel model)
        {
            var checkoutAttribute = await _checkoutAttributeService.GetCheckoutAttributeById(model.CheckoutAttributeId);

            var cav = checkoutAttribute.CheckoutAttributeValues.Where(x => x.Id == model.Id).FirstOrDefault();
            if (cav == null)
                //No checkout attribute value found with the specified id
                return RedirectToAction("List");

            model.PrimaryStoreCurrencyCode = (await _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId)).CurrencyCode;
            model.BaseWeightIn = (await _measureService.GetMeasureWeightById(_measureSettings.BaseWeightId)).Name;

            if (checkoutAttribute.AttributeControlType == AttributeControlType.ColorSquares)
            {
                //ensure valid color is chosen/entered
                if (String.IsNullOrEmpty(model.ColorSquaresRgb))
                    ModelState.AddModelError("", "Color is required");
            }

            if (ModelState.IsValid)
            {
                await _checkoutAttributeViewModelService.UpdateCheckoutAttributeValueModel(checkoutAttribute, cav, model);
                ViewBag.RefreshPage = true;
                return View(model);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        //delete
        [HttpPost]
        public async Task<IActionResult> ValueDelete(string id, string checkoutAttributeId)
        {
            var checkoutAttribute = await _checkoutAttributeService.GetCheckoutAttributeById(checkoutAttributeId);
            var cav = checkoutAttribute.CheckoutAttributeValues.Where(x => x.Id == id).FirstOrDefault();
            if (cav == null)
                throw new ArgumentException("No checkout attribute value found with the specified id");

            if (ModelState.IsValid)
            {
                checkoutAttribute.CheckoutAttributeValues.Remove(cav);
                await _checkoutAttributeService.UpdateCheckoutAttribute(checkoutAttribute);
                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }
        #endregion
    }
}
