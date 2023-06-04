using ForeverNote.Core;
using ForeverNote.Web.Framework.Components;
using ForeverNote.Services.Messages;
using ForeverNote.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ForeverNote.Web.ViewComponents
{
    public class PopupActionViewComponent : BaseViewComponent
    {
        private readonly IPopupService _popupService;
        private readonly IWorkContext _workContext;

        public PopupActionViewComponent(IPopupService popupService, IWorkContext workContext)
        {
            _popupService = popupService;
            _workContext = workContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _popupService.GetActivePopupByCustomerId(_workContext.CurrentCustomer.Id);
            if (result == null)
                return Content("");

            var model = new PopupModel();
            model.Id = result.Id;
            model.Body = result.Body;
            model.Name = result.Name;
            model.PopupType = (Core.Domain.Messages.PopupType)result.PopupTypeId;
            model.CustomerActionId = result.CustomerActionId;

            await _popupService.MovepopupToArchive(result.Id, _workContext.CurrentCustomer.Id);

            return View(model);
        }
    }
}