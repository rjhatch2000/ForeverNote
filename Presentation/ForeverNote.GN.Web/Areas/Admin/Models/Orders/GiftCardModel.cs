using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Orders;
using System;

namespace ForeverNote.Web.Areas.Admin.Models.Orders
{
    [Validator(typeof(GiftCardValidator))]
    public partial class GiftCardModel: BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.GiftCards.Fields.GiftCardType")]
        public int GiftCardTypeId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.Fields.Order")]
        public string PurchasedWithOrderId { get; set; }
        public int PurchasedWithOrderNumber { get; set; }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.Fields.Amount")]
        public decimal Amount { get; set; }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.Fields.Amount")]
        public string AmountStr { get; set; }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.Fields.RemainingAmount")]
        public string RemainingAmountStr { get; set; }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.Fields.IsGiftCardActivated")]
        public bool IsGiftCardActivated { get; set; }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.Fields.GiftCardCouponCode")]
        
        public string GiftCardCouponCode { get; set; }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.Fields.RecipientName")]
        
        public string RecipientName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.Fields.RecipientEmail")]
        
        public string RecipientEmail { get; set; }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.Fields.SenderName")]
        
        public string SenderName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.Fields.SenderEmail")]
        
        public string SenderEmail { get; set; }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.Fields.Message")]
        
        public string Message { get; set; }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.Fields.IsRecipientNotified")]
        public bool IsRecipientNotified { get; set; }

        [ForeverNoteResourceDisplayName("Admin.GiftCards.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        public string PrimaryStoreCurrencyCode { get; set; }

        #region Nested classes

        public partial class GiftCardUsageHistoryModel : BaseForeverNoteEntityModel
        {
            [ForeverNoteResourceDisplayName("Admin.GiftCards.History.UsedValue")]
            public string UsedValue { get; set; }

            [ForeverNoteResourceDisplayName("Admin.GiftCards.History.Order")]
            public string OrderId { get; set; }
            public int OrderNumber { get; set; }

            [ForeverNoteResourceDisplayName("Admin.GiftCards.History.CreatedOn")]
            public DateTime CreatedOn { get; set; }
        }

        #endregion
    }
}