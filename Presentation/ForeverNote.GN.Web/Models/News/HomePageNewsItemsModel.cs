using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Models.Media;
using System;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.News
{
    public partial class HomePageNewsItemsModel : BaseForeverNoteModel
    {
        public HomePageNewsItemsModel()
        {
            NewsItems = new List<NewsItemModel>();
        }

        public IList<NewsItemModel> NewsItems { get; set; }

        public class NewsItemModel : BaseForeverNoteModel
        {
            public NewsItemModel()
            {
                PictureModel = new PictureModel();
            }

            public string Id { get; set; }
            public string SeName { get; set; }
            public string Title { get; set; }
            public PictureModel PictureModel { get; set; }
            public string Short { get; set; }
            public string Full { get; set; }
            public DateTime CreatedOn { get; set; }
        }
    }
}