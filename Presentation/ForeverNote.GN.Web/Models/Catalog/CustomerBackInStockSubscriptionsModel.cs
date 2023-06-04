using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Models.Common;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Catalog
{
    public partial class CustomerBackInStockSubscriptionsModel
    {
        public CustomerBackInStockSubscriptionsModel()
        {
            Subscriptions = new List<BackInStockSubscriptionModel>();
        }

        public IList<BackInStockSubscriptionModel> Subscriptions { get; set; }
        public PagerModel PagerModel { get; set; }

        #region Nested classes

        public partial class BackInStockSubscriptionModel : BaseForeverNoteEntityModel
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public string AttributeDescription { get; set; }
            public string SeName { get; set; }
        }

        #endregion
    }
}