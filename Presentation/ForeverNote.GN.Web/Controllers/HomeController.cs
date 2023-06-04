using Microsoft.AspNetCore.Mvc;

namespace ForeverNote.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        public virtual IActionResult Index() => View();
    }
}
