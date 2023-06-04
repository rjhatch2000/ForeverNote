using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using System;

namespace ForeverNote.Web.Areas.Admin.Models.Orders
{
    public partial class RecurringPaymentModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.RecurringPayments.Fields.ID")]
        public override string Id { get; set; }

        [ForeverNoteResourceDisplayName("Admin.RecurringPayments.Fields.CycleLength")]
        public int CycleLength { get; set; }

        [ForeverNoteResourceDisplayName("Admin.RecurringPayments.Fields.CyclePeriod")]
        public int CyclePeriodId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.RecurringPayments.Fields.CyclePeriod")]
        public string CyclePeriodStr { get; set; }

        [ForeverNoteResourceDisplayName("Admin.RecurringPayments.Fields.TotalCycles")]
        public int TotalCycles { get; set; }

        [ForeverNoteResourceDisplayName("Admin.RecurringPayments.Fields.StartDate")]
        public string StartDate { get; set; }

        [ForeverNoteResourceDisplayName("Admin.RecurringPayments.Fields.IsActive")]
        public bool IsActive { get; set; }

        [ForeverNoteResourceDisplayName("Admin.RecurringPayments.Fields.NextPaymentDate")]
        public string NextPaymentDate { get; set; }

        [ForeverNoteResourceDisplayName("Admin.RecurringPayments.Fields.CyclesRemaining")]
        public int CyclesRemaining { get; set; }

        [ForeverNoteResourceDisplayName("Admin.RecurringPayments.Fields.InitialOrder")]
        public string InitialOrderId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.RecurringPayments.Fields.Customer")]
        public string CustomerId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.RecurringPayments.Fields.Customer")]
        public string CustomerEmail { get; set; }

        [ForeverNoteResourceDisplayName("Admin.RecurringPayments.Fields.PaymentType")]
        public string PaymentType { get; set; }
        
        public bool CanCancelRecurringPayment { get; set; }

        #region Nested classes


        public partial class RecurringPaymentHistoryModel : BaseForeverNoteEntityModel
        {
            [ForeverNoteResourceDisplayName("Admin.RecurringPayments.History.Order")]
            public string OrderId { get; set; }
            public int OrderNumber { get; set; }

            public string RecurringPaymentId { get; set; }

            [ForeverNoteResourceDisplayName("Admin.RecurringPayments.History.OrderStatus")]
            public string OrderStatus { get; set; }

            [ForeverNoteResourceDisplayName("Admin.RecurringPayments.History.PaymentStatus")]
            public string PaymentStatus { get; set; }

            [ForeverNoteResourceDisplayName("Admin.RecurringPayments.History.ShippingStatus")]
            public string ShippingStatus { get; set; }

            [ForeverNoteResourceDisplayName("Admin.RecurringPayments.History.CreatedOn")]
            public DateTime CreatedOn { get; set; }
        }

        #endregion
    }
}