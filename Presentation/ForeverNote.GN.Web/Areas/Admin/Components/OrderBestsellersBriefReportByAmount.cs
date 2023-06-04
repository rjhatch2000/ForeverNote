using ForeverNote.Web.Framework.Components;
using ForeverNote.Services.Security;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ForeverNote.Web.Areas.Admin.Components
{
    public class OrderBestsellersBriefReportByAmountViewComponent : BaseViewComponent
    {
        private readonly IPermissionService _permissionService;

        public OrderBestsellersBriefReportByAmountViewComponent(IPermissionService permissionService)
        {
            this._permissionService = permissionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!await _permissionService.Authorize(StandardPermissionProvider.ManageOrders))
                return Content("");

            return View();
        }
    }
}