using Microsoft.AspNetCore.Mvc;

namespace ForeverNote.Web.Framework.Mvc
{
    public class NullJsonResult : JsonResult
    {
        public NullJsonResult() : base(null)
        {
        }
    }
}
