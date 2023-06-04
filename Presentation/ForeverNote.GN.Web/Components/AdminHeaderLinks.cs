using ForeverNote.Core;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Web.Framework.Components;
using ForeverNote.Web.Framework.UI;
using ForeverNote.Services.Security;
using ForeverNote.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ForeverNote.Web.ViewComponents
{
    public class AdminHeaderLinksViewComponent : BaseViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly IPageHeadBuilder _pageHeadBuilder;
        private readonly IPermissionService _permissionService;

        private readonly CustomerSettings _customerSettings;

        public AdminHeaderLinksViewComponent(IPageHeadBuilder pageHeadBuilder,
            IPermissionService permissionService,
            IWorkContext workContext,
            CustomerSettings customerSettings)
        {
            _pageHeadBuilder = pageHeadBuilder;
            _workContext = workContext;
            _customerSettings = customerSettings;
            _permissionService = permissionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new AdminHeaderLinksModel {
                ImpersonatedCustomerEmailUsername = _workContext.CurrentCustomer.IsRegistered() ? (_customerSettings.UsernamesEnabled ? _workContext.CurrentCustomer.Username : _workContext.CurrentCustomer.Email) : "",
                IsCustomerImpersonated = _workContext.OriginalCustomerIfImpersonated != null,
                DisplayAdminLink = await _permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel),
                EditPageUrl = _pageHeadBuilder.GetEditPageUrl()
            };

            if (!model.DisplayAdminLink && !model.IsCustomerImpersonated)
                return Content("");
            return View(model);
        }
    }
}