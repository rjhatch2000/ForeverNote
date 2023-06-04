﻿using ForeverNote.Core;
using ForeverNote.Core.Configuration;
using ForeverNote.Core.Domain;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Media;
using ForeverNote.Core.Domain.Tax;
using ForeverNote.Web.Framework.Localization;
using ForeverNote.Web.Framework.Mvc.Filters;
using ForeverNote.Web.Framework.Security.Captcha;
using ForeverNote.Web.Framework.Themes;
using ForeverNote.Services.Common;
using ForeverNote.Services.Directory;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Media;
using ForeverNote.Services.Messages;
using ForeverNote.Services.Stores;
using ForeverNote.Web.Commands.Models.Common;
using ForeverNote.Web.Features.Models.Common;
using ForeverNote.Web.Models.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ForeverNote.Web.Controllers
{
    public partial class CommonController : BasePublicController
    {
        #region Fields
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IMediator _mediator;
        private readonly CaptchaSettings _captchaSettings;

        #endregion

        #region Constructors

        public CommonController(
            ILocalizationService localizationService,
            IWorkContext workContext,
            IStoreContext storeContext,
            IMediator mediator,
            CaptchaSettings captchaSettings)
        {
            _localizationService = localizationService;
            _workContext = workContext;
            _storeContext = storeContext;
            _mediator = mediator;
            _captchaSettings = captchaSettings;
        }

        #endregion

        #region Methods

        //page not found
        public virtual IActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            Response.ContentType = "text/html";
            return View();
        }

        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual async Task<IActionResult> SetLanguage(
            [FromServices] ILanguageService languageService,
            [FromServices] ForeverNoteConfig config,
            string langid, string returnUrl = "")
        {

            var language = await languageService.GetLanguageById(langid);
            if (!language?.Published ?? false)
                language = _workContext.WorkingLanguage;

            //home page
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            //language part in URL
            if (config.SeoFriendlyUrlsForLanguagesEnabled)
            {
                //remove current language code if it's already localized URL
                if (await returnUrl.IsLocalizedUrlAsync(languageService, this.Request.PathBase, true))
                    returnUrl = returnUrl.RemoveLanguageSeoCodeFromUrl(this.Request.PathBase, true);

                //and add code of passed language
                returnUrl = returnUrl.AddLanguageSeoCodeToUrl(this.Request.PathBase, true, language);
            }

            await _workContext.SetWorkingLanguage(language);

            return Redirect(returnUrl);
        }

        //helper method to redirect users.
        public virtual IActionResult InternalRedirect(string url, bool permanentRedirect)
        {           
            //ensure it's invoked from our GenericPathRoute class
            if (HttpContext.Items["ForeverNote.RedirectFromGenericPathRoute"] == null ||
                !Convert.ToBoolean(HttpContext.Items["ForeverNote.RedirectFromGenericPathRoute"]))
            {
                url = Url.RouteUrl("HomePage");
                permanentRedirect = false;
            }

            //home page
            if (string.IsNullOrEmpty(url))
            {
                url = Url.RouteUrl("HomePage");
                permanentRedirect = false;
            }

            //prevent open redirection attack
            if (!Url.IsLocalUrl(url))
            {
                url = Url.RouteUrl("HomePage");
                permanentRedirect = false;
            }

            url = Uri.EscapeUriString(WebUtility.UrlDecode(url));

            if (permanentRedirect)
                return RedirectPermanent(url);
            return Redirect(url);
        }

        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual async Task<IActionResult> SetCurrency(
            [FromServices] ICurrencyService currencyService,
            string customerCurrency, string returnUrl = "")
        {
            var currency = await currencyService.GetCurrencyById(customerCurrency);
            if (currency != null)
                await _workContext.SetWorkingCurrency(currency);

            //home page
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            return Redirect(returnUrl);
        }

        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual async Task<IActionResult> SetStore(
            [FromServices] IStoreService storeService,
            [FromServices] CommonSettings commonSettings,
            string store, string returnUrl = "")
        {
            var currentstoreid = _storeContext.CurrentStore.Id;
            if (currentstoreid != store)
            {
                if (commonSettings.AllowToSelectStore)
                {
                    var selectedstore = await storeService.GetStoreById(store);
                    if (selectedstore != null)
                        await _storeContext.SetStoreCookie(store);
                }
            }
            var prevStore = await storeService.GetStoreById(currentstoreid);
            var currStore = await storeService.GetStoreById(store);

            if (prevStore != null && currStore != null)
            {
                if (prevStore.Url != currStore.Url)
                {
                    return Redirect(currStore.SslEnabled ? currStore.SecureUrl : currStore.Url);
                }
            }

            //home page
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            return Redirect(returnUrl);
        }

        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual async Task<IActionResult> SetTaxType(int customerTaxType, string returnUrl = "")
        {
            var taxDisplayType = (TaxDisplayType)Enum.ToObject(typeof(TaxDisplayType), customerTaxType);
            await _workContext.SetTaxDisplayType(taxDisplayType);

            //home page
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            return Redirect(returnUrl);
        }

        //contact us page
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        public virtual async Task<IActionResult> ContactUs()
        {
            var model = await _mediator.Send(new ContactUsCommand() {
                Customer = _workContext.CurrentCustomer,
                Language = _workContext.WorkingLanguage,
                Store = _storeContext.CurrentStore
            });
            return View(model);
        }

        [HttpPost, ActionName("ContactUs")]
        [AutoValidateAntiforgeryToken]
        [ValidateCaptcha]
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        public virtual async Task<IActionResult> ContactUsSend(ContactUsModel model, IFormCollection form, bool captchaValid)
        {
            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnContactUsPage && !captchaValid)
            {
                ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            }

            var result = await _mediator.Send(new ContactUsSendCommand() {
                CaptchaValid = captchaValid,
                Form = form,
                Model = model,
                Store = _storeContext.CurrentStore
            });

            if (result.errors.Any())
            {
                foreach (var item in result.errors)
                {
                    ModelState.AddModelError("", item);
                }
            }
            else
            {
                model = result.model;
                return View(model);
            }

            model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnContactUsPage;

            return View(model);
        }


        //sitemap page
        public virtual async Task<IActionResult> Sitemap([FromServices] CommonSettings commonSettings)
        {
            if (!commonSettings.SitemapEnabled)
                return RedirectToRoute("HomePage");

            var model = await _mediator.Send(new GetSitemap() {
                Customer = _workContext.CurrentCustomer,
                Language = _workContext.WorkingLanguage,
                Store = _storeContext.CurrentStore
            });
            return View(model);
        }

        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        public virtual async Task<IActionResult> SitemapXml(int? id, 
            [FromServices] CommonSettings commonSettings)
        {
            if (!commonSettings.SitemapEnabled)
                return RedirectToRoute("HomePage");

            var siteMap = await _mediator.Send(new GetSitemapXml() {
                Id = id,
                Customer = _workContext.CurrentCustomer,
                Language = _workContext.WorkingLanguage,
                Store = _storeContext.CurrentStore,
                UrlHelper = Url,
            });

            return Content(siteMap, "text/xml");
        }

        public virtual async Task<IActionResult> SetStoreTheme(
            [FromServices] IThemeContext themeContext, string themeName, string returnUrl = "")
        {
            await themeContext.SetWorkingTheme(themeName);

            //home page
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            return Redirect(returnUrl);
        }


        [HttpPost]
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual async Task<IActionResult> EuCookieLawAccept(
            [FromServices] StoreInformationSettings storeInformationSettings,
            [FromServices] IGenericAttributeService genericAttributeService)
        {
            if (!storeInformationSettings.DisplayEuCookieLawWarning)
                //disabled
                return Json(new { stored = false });

            //save setting
            await genericAttributeService.SaveAttribute(_workContext.CurrentCustomer, SystemCustomerAttributeNames.EuCookieLawAccepted, true, _storeContext.CurrentStore.Id);
            return Json(new { stored = true });
        }

        //robots.txt file
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual async Task<IActionResult> RobotsTextFile()
        {
            var sb = await _mediator.Send(new GetRobotsTextFile());
            return Content(sb, "text/plain");
        }

        public virtual IActionResult GenericUrl()
        {
            //seems that no entity was found
            return InvokeHttp404();
        }

        //store is closed
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        public virtual IActionResult StoreClosed() => View();

        [HttpPost]
        public virtual async Task<IActionResult> ContactAttributeChange(IFormCollection form)
        {
            var result = await _mediator.Send(new ContactAttributeChangeCommand() {
                Form = form,
                Customer = _workContext.CurrentCustomer,
                Store = _storeContext.CurrentStore
            });
            return Json(new
            {
                enabledattributeids = result.enabledAttributeIds.ToArray(),
                disabledattributeids = result.disabledAttributeIds.ToArray()
            });
        }

        [HttpPost]
        public virtual async Task<IActionResult> UploadFileContactAttribute(string attributeId, [FromServices] IDownloadService downloadService,
            [FromServices] IContactAttributeService contactAttributeService)
        {
            var attribute = await contactAttributeService.GetContactAttributeById(attributeId);
            if (attribute == null || attribute.AttributeControlType != AttributeControlType.FileUpload)
            {
                return Json(new
                {
                    success = false,
                    downloadGuid = Guid.Empty,
                });
            }
            var form = await HttpContext.Request.ReadFormAsync();
            var httpPostedFile = form.Files.FirstOrDefault();
            if (httpPostedFile == null)
            {
                return Json(new
                {
                    success = false,
                    message = "No file uploaded",
                    downloadGuid = Guid.Empty,
                });
            }

            var fileBinary = httpPostedFile.GetDownloadBits();

            var qqFileNameParameter = "qqfilename";
            var fileName = httpPostedFile.FileName;
            if (String.IsNullOrEmpty(fileName) && form.ContainsKey(qqFileNameParameter))
                fileName = form[qqFileNameParameter].ToString();
            //remove path (passed in IE)
            fileName = Path.GetFileName(fileName);

            var contentType = httpPostedFile.ContentType;

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            if (attribute.ValidationFileMaximumSize.HasValue)
            {
                //compare in bytes
                var maxFileSizeBytes = attribute.ValidationFileMaximumSize.Value * 1024;
                if (fileBinary.Length > maxFileSizeBytes)
                {
                    //when returning JSON the mime-type must be set to text/plain
                    //otherwise some browsers will pop-up a "Save As" dialog.
                    return Json(new
                    {
                        success = false,
                        message = string.Format(_localizationService.GetResource("ShoppingCart.MaximumUploadedFileSize"), attribute.ValidationFileMaximumSize.Value),
                        downloadGuid = Guid.Empty,
                    });
                }
            }

            var download = new Download {
                DownloadGuid = Guid.NewGuid(),
                UseDownloadUrl = false,
                DownloadUrl = "",
                DownloadBinary = fileBinary,
                ContentType = contentType,
                //we store filename without extension for downloads
                Filename = Path.GetFileNameWithoutExtension(fileName),
                Extension = fileExtension,
                IsNew = true
            };

            await downloadService.InsertDownload(download);

            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new
            {
                success = true,
                message = _localizationService.GetResource("ShoppingCart.FileUploaded"),
                downloadUrl = Url.Action("GetFileUpload", "Download", new { downloadId = download.DownloadGuid }),
                downloadGuid = download.DownloadGuid,
            });
        }


        [HttpPost, ActionName("PopupInteractiveForm")]
        public virtual async Task<IActionResult> PopupInteractiveForm(IFormCollection formCollection)
        {
            var result = await _mediator.Send(new PopupInteractiveCommand() { Form = formCollection });
            return Json(new
            {
                success = result.Any(),
                errors = result
            });
        }

        #endregion
    }
}
