﻿using ForeverNote.Core;
using ForeverNote.Core.Domain.Vendors;
using ForeverNote.Web.Framework.Controllers;
using ForeverNote.Web.Framework.Kendoui;
using ForeverNote.Web.Framework.Mvc;
using ForeverNote.Web.Framework.Mvc.Filters;
using ForeverNote.Web.Framework.Security.Authorization;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Security;
using ForeverNote.Services.Seo;
using ForeverNote.Services.Vendors;
using ForeverNote.Web.Areas.Admin.Extensions;
using ForeverNote.Web.Areas.Admin.Models.Vendors;
using ForeverNote.Web.Areas.Admin.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.Vendors)]
    public partial class VendorController : BaseAdminController
    {
        #region Fields
        private readonly IVendorViewModelService _vendorViewModelService;
        private readonly ILocalizationService _localizationService;
        private readonly IVendorService _vendorService;
        private readonly ILanguageService _languageService;
        #endregion

        #region Constructors

        public VendorController(
            IVendorViewModelService vendorViewModelService,
            ILocalizationService localizationService,
            IVendorService vendorService,
            ILanguageService languageService)
        {
            this._vendorViewModelService = vendorViewModelService;
            this._localizationService = localizationService;
            this._vendorService = vendorService;
            this._languageService = languageService;
        }

        #endregion

        #region Methods

        //list
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List()
        {
            var model = new VendorListModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, VendorListModel model)
        {
            var vendors = await _vendorService.GetAllVendors(model.SearchName, command.Page - 1, command.PageSize, true);
            var gridModel = new DataSourceResult
            {
                Data = vendors.Select(x =>
                {
                    var vendorModel = x.ToModel();
                    return vendorModel;
                }),
                Total = vendors.TotalCount,
            };
            return Json(gridModel);
        }

        //create
        public async Task<IActionResult> Create()
        {
            var model = await _vendorViewModelService.PrepareVendorModel();
            //locales
            await AddLocales(_languageService, model.Locales);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public async Task<IActionResult> Create(VendorModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var vendor = await _vendorViewModelService.InsertVendorModel(model);
                SuccessNotification(_localizationService.GetResource("Admin.Vendors.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = vendor.Id }) : RedirectToAction("List");
            }
            //prepare address model
            await _vendorViewModelService.PrepareVendorAddressModel(model, null);
            //discounts
            await _vendorViewModelService.PrepareDiscountModel(model, null, true);
            //stores
            await _vendorViewModelService.PrepareStore(model);

            //If we got this far, something failed, redisplay form
            return View(model);
        }


        //edit
        public async Task<IActionResult> Edit(string id)
        {
            var vendor = await _vendorService.GetVendorById(id);
            if (vendor == null || vendor.Deleted)
                //No vendor found with the specified id
                return RedirectToAction("List");

            var model = vendor.ToModel();
            //locales
            await AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = vendor.GetLocalized(x => x.Name, languageId, false, false);
                locale.Description = vendor.GetLocalized(x => x.Description, languageId, false, false);
                locale.MetaKeywords = vendor.GetLocalized(x => x.MetaKeywords, languageId, false, false);
                locale.MetaDescription = vendor.GetLocalized(x => x.MetaDescription, languageId, false, false);
                locale.MetaTitle = vendor.GetLocalized(x => x.MetaTitle, languageId, false, false);
                locale.SeName = vendor.GetSeName(languageId, false, false);
            });
            //discounts
            await _vendorViewModelService.PrepareDiscountModel(model, vendor, false);
            //associated customer emails
            model.AssociatedCustomers = await _vendorViewModelService.AssociatedCustomers(vendor.Id);
            //prepare address model
            await _vendorViewModelService.PrepareVendorAddressModel(model, vendor);
            //stores
            await _vendorViewModelService.PrepareStore(model);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(VendorModel model, bool continueEditing)
        {
            var vendor = await _vendorService.GetVendorById(model.Id);
            if (vendor == null || vendor.Deleted)
                //No vendor found with the specified id
                return RedirectToAction("List");
            
            if (ModelState.IsValid)
            {
                vendor = await _vendorViewModelService.UpdateVendorModel(vendor, model);

                SuccessNotification(_localizationService.GetResource("Admin.Vendors.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = vendor.Id });
                }
                return RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            //discounts
            await _vendorViewModelService.PrepareDiscountModel(model, vendor, true);
            //prepare address model
            await _vendorViewModelService.PrepareVendorAddressModel(model, vendor);
            //associated customer emails
            model.AssociatedCustomers = await _vendorViewModelService.AssociatedCustomers(vendor.Id);
            //stores
            await _vendorViewModelService.PrepareStore(model);

            return View(model);
        }

        //delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var vendor = await _vendorService.GetVendorById(id);
            if (vendor == null)
                //No vendor found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _vendorViewModelService.DeleteVendor(vendor);
                SuccessNotification(_localizationService.GetResource("Admin.Vendors.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = vendor.Id });
        }

        #endregion

        #region Vendor notes

        [HttpPost]
        public async Task<IActionResult> VendorNotesSelect(string vendorId, DataSourceRequest command)
        {
            var vendor = await _vendorService.GetVendorById(vendorId);
            if (vendor == null)
                throw new ArgumentException("No vendor found with the specified id");

            var vendorNoteModels = _vendorViewModelService.PrepareVendorNote(vendor);
            var gridModel = new DataSourceResult
            {
                Data = vendorNoteModels,
                Total = vendorNoteModels.Count
            };

            return Json(gridModel);
        }

        public async Task<IActionResult> VendorNoteAdd(string vendorId, string message)
        {
            if (ModelState.IsValid)
            {
                var result = await _vendorViewModelService.InsertVendorNote(vendorId, message);
                return Json(new { Result = result });
            }
            return ErrorForKendoGridJson(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> VendorNoteDelete(string id, string vendorId)
        {
            if (ModelState.IsValid)
            {
                await _vendorViewModelService.DeleteVendorNote(id, vendorId);
                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }

        #endregion

        #region Reviews

        [HttpPost]
        public async Task<IActionResult> Reviews(DataSourceRequest command, string vendorId, [FromServices] IWorkContext workContext)
        {
            var vendor = await _vendorService.GetVendorById(vendorId);
            if (vendor == null)
                throw new ArgumentException("No vendor found with the specified id");

            //a vendor should have access only to his own profile
            if (workContext.CurrentVendor != null && vendor.Id != workContext.CurrentVendor.Id)
                return Content("This is not your vendor");

            var vendorReviews = await _vendorService.GetAllVendorReviews("", null,
                null, null, "", vendorId, command.Page - 1, command.PageSize);
            var items = new List<VendorReviewModel>();
            foreach (var item in vendorReviews)
            {
                var m = new VendorReviewModel();
                await _vendorViewModelService.PrepareVendorReviewModel(m, item, false, true);
                items.Add(m);
            }
            var gridModel = new DataSourceResult
            {
                Data = items,
                Total = vendorReviews.TotalCount,
            };

            return Json(gridModel);
        }

        #endregion

    }
}
