using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Models.Common;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Boards
{
    public partial class CustomerForumSubscriptionsModel
    {
        public CustomerForumSubscriptionsModel()
        {
            ForumSubscriptions = new List<ForumSubscriptionModel>();
        }

        public IList<ForumSubscriptionModel> ForumSubscriptions { get; set; }
        public PagerModel PagerModel { get; set; }

        #region Nested classes

        public partial class ForumSubscriptionModel : BaseForeverNoteEntityModel
        {
            public string ForumId { get; set; }
            public string ForumTopicId { get; set; }
            public bool TopicSubscription { get; set; }
            public string Title { get; set; }
            public string Slug { get; set; }
        }

        #endregion
    }
}