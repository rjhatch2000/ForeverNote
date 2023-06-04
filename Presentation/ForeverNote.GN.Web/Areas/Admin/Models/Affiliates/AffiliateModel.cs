using System;
using ForeverNote.Web.Areas.Admin.Models.Common;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Framework.Mvc.ModelBinding;

namespace ForeverNote.Web.Areas.Admin.Models.Affiliates
{
    public partial class AffiliateModel : BaseForeverNoteEntityModel
    {
        public AffiliateModel()
        {
            Address = new AddressModel();
        }

        [ForeverNoteResourceDisplayName("Admin.Affiliates.Fields.ID")]
        public override string Id { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Affiliates.Fields.URL")]
        public string Url { get; set; }


        [ForeverNoteResourceDisplayName("Admin.Affiliates.Fields.AdminComment")]
        
        public string AdminComment { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Affiliates.Fields.FriendlyUrlName")]
        
        public string FriendlyUrlName { get; set; }
        
        [ForeverNoteResourceDisplayName("Admin.Affiliates.Fields.Active")]
        public bool Active { get; set; }

        public AddressModel Address { get; set; }

        #region Nested classes
        
        public partial class AffiliatedOrderModel : BaseForeverNoteEntityModel
        {
            [ForeverNoteResourceDisplayName("Admin.Affiliates.Orders.Order")]
            public override string Id { get; set; }
            public int OrderNumber { get; set; }

            public string OrderCode { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Affiliates.Orders.OrderStatus")]
            public string OrderStatus { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Affiliates.Orders.PaymentStatus")]
            public string PaymentStatus { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Affiliates.Orders.ShippingStatus")]
            public string ShippingStatus { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Affiliates.Orders.OrderTotal")]
            public string OrderTotal { get; set; }

            [ForeverNoteResourceDisplayName("Admin.Affiliates.Orders.CreatedOn")]
            public DateTime CreatedOn { get; set; }
        }

        public partial class AffiliatedCustomerModel : BaseForeverNoteEntityModel
        {
            [ForeverNoteResourceDisplayName("Admin.Affiliates.Customers.Name")]
            public string Name { get; set; }
        }

        #endregion
    }
}