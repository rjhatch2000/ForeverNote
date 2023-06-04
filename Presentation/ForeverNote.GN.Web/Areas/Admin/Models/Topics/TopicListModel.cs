using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Topics
{
    public partial class TopicListModel : BaseForeverNoteModel
    {
        public TopicListModel()
        {
            AvailableStores = new List<SelectListItem>();
        }
        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.List.Name")]
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ContentManagement.Topics.List.SearchStore")]
        public string SearchStoreId { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
    }
}