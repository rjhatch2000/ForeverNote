﻿using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Cms
{
    public partial class RenderWidgetModel : BaseForeverNoteModel
    {
        public string WidgetViewComponentName { get; set; }
        public object WidgetViewComponentArguments { get; set; }
    }
}