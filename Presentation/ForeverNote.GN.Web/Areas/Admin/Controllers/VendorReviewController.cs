﻿using ForeverNote.Core;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Web.Framework.Kendoui;
using ForeverNote.Web.Framework.Mvc.Filters;
using ForeverNote.Web.Framework.Security.Authorization;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Security;
using ForeverNote.Services.Vendors;
using ForeverNote.Web.Areas.Admin.Models.Customers;
using ForeverNote.Web.Areas.Admin.Models.Vendors;
using ForeverNote.Web.Areas.Admin.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.VendorReviews)]
    public partial class VendorReviewController : BaseAdminController
    {
        #region Fields
        private readonly IVendorViewModelService _vendorViewModelService;
        private readonly IVendorService _vendorService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        #endregion Fields

        #region Constructors

        public VendorReviewController(
            IVendorViewModelService vendorViewModelService,
            IVendorService vendorService,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            this._vendorViewModelService = vendorViewModelService;
            this._vendorService = vendorService;
            this._localizationService = localizationService;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        //list
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List()
        {
            var model = new VendorReviewListModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, VendorReviewListModel model)
        {
            var vendorId = string.Empty;
            //vendor
            if (_workContext.CurrentVendor != null)
            {
                vendorId = _workContext.CurrentVendor.Id;
            }
            //admin
            else if (_workContext.CurrentCustomer.IsAdmin())
            {
                vendorId = model.SearchVendorId;
            }

            model.SearchVendorId = vendorId;
            var (vendorReviewModels, totalCount) = await _vendorViewModelService.PrepareVendorReviewModel(model, command.Page, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = vendorReviewModels.ToList(),
                Total = totalCount,
            };

            return Json(gridModel);
        }

        //edit
        public async Task<IActionResult> Edit(string id)
        {
            var vendorReview = await _vendorService.GetVendorReviewById(id);

            if (vendorReview == null)
                //No vendor review found with the specified id
                return RedirectToAction("List");

            var model = new VendorReviewModel();
            await _vendorViewModelService.PrepareVendorReviewModel(model, vendorReview, false, false);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(VendorReviewModel model, bool continueEditing)
        {
            var vendorReview = await _vendorService.GetVendorReviewById(model.Id);
            if (vendorReview == null)
                //No vendor review found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                vendorReview = await _vendorViewModelService.UpdateVendorReviewModel(vendorReview, model);
                SuccessNotification(_localizationService.GetResource("Admin.VendorReviews.Updated"));
                return continueEditing ? RedirectToAction("Edit", new { id = vendorReview.Id, VendorId = vendorReview.VendorId }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            await _vendorViewModelService.PrepareVendorReviewModel(model, vendorReview, true, false);
            return View(model);
        }

        //delete
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var vendorReview = await _vendorService.GetVendorReviewById(id);
            if (vendorReview == null)
                //No vendor review found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                await _vendorViewModelService.DeleteVendorReview(vendorReview);

                SuccessNotification(_localizationService.GetResource("Admin.VendorReviews.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> ApproveSelected(ICollection<string> selectedIds)
        {
            if (selectedIds != null)
            {
                await _vendorViewModelService.ApproveVendorReviews(selectedIds.ToList());
            }
            return Json(new { Result = true });
        }

        [HttpPost]
        public async Task<IActionResult> DisapproveSelected(ICollection<string> selectedIds)
        {
            if (selectedIds != null)
            {
                await _vendorViewModelService.DisapproveVendorReviews(selectedIds.ToList());
            }

            return Json(new { Result = true });
        }

        public async Task<IActionResult> VendorSearchAutoComplete(string term)
        {
            const int searchTermMinimumLength = 3;
            if (String.IsNullOrWhiteSpace(term) || term.Length < searchTermMinimumLength)
                return Content("");

            var vendors = await _vendorService.SearchVendors(
                keywords: term);

            var result = (from p in vendors
                          select new
                          {
                              label = p.Name,
                              vendorid = p.Id
                          })
                .ToList();
            return Json(result);
        }
        #endregion
    }
}