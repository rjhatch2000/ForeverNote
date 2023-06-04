﻿using ForeverNote.Core.Domain.Orders;
using ForeverNote.Web.Framework.Mvc.Models;
using System;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Orders
{
    public partial class CustomerOrderListModel : BaseForeverNoteModel
    {
        public CustomerOrderListModel()
        {
            Orders = new List<OrderDetailsModel>();
            RecurringOrders = new List<RecurringOrderModel>();
            CancelRecurringPaymentErrors = new List<string>();
        }

        public IList<OrderDetailsModel> Orders { get; set; }
        public IList<RecurringOrderModel> RecurringOrders { get; set; }
        public IList<string> CancelRecurringPaymentErrors { get; set; }


        #region Nested classes

        public partial class OrderDetailsModel : BaseForeverNoteEntityModel
        {
            public string OrderTotal { get; set; }
            public bool IsReturnRequestAllowed { get; set; }
            public OrderStatus OrderStatusEnum { get; set; }
            public string OrderStatus { get; set; }
            public string PaymentStatus { get; set; }
            public string ShippingStatus { get; set; }
            public DateTime CreatedOn { get; set; }
            public int OrderNumber { get; set; }
            public string OrderCode { get; set; }
        }

        public partial class RecurringOrderModel : BaseForeverNoteEntityModel
        {
            public string StartDate { get; set; }
            public string CycleInfo { get; set; }
            public string NextPayment { get; set; }
            public int TotalCycles { get; set; }
            public int CyclesRemaining { get; set; }
            public string InitialOrderId { get; set; }
            public bool CanCancel { get; set; }
        }

        #endregion
    }
}