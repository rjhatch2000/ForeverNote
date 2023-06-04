﻿using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Models.Media
{
    public partial class PictureModel : BaseForeverNoteEntityModel
    {
        public string ImageUrl { get; set; }
        public string ThumbImageUrl { get; set; }

        public string FullSizeImageUrl { get; set; }

        public string Title { get; set; }

        public string AlternateText { get; set; }
    }
}