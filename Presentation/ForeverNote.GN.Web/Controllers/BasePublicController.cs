using ForeverNote.Web.Framework.Controllers;
using ForeverNote.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ForeverNote.Web.Controllers
{
    [CheckAccessPublicStore]
    [CheckAccessClosedStore]
    [CheckLanguageSeoCode]
    [CheckAffiliate]
    public abstract partial class BasePublicController : BaseController
    {
        protected virtual IActionResult InvokeHttp404()
        {
            Response.StatusCode = 404;
            return new EmptyResult();
        }
    }
}
