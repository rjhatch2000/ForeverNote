using ForeverNote.Web.Framework.Components;
using ForeverNote.Web.Models.Checkout;
using Microsoft.AspNetCore.Mvc;

namespace ForeverNote.Web.ViewComponents
{
    public class CheckoutProgressViewComponent : BaseViewComponent
    {
        public IViewComponentResult Invoke(CheckoutProgressStep step)
        {
            var model = new CheckoutProgressModel { CheckoutProgressStep = step };
            return View(model);

        }
    }
}