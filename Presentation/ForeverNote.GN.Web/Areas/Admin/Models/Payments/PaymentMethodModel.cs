using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;

namespace ForeverNote.Web.Areas.Admin.Models.Payments
{
    public partial class PaymentMethodModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.FriendlyName")]
        
        public string FriendlyName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.SystemName")]
        
        public string SystemName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.IsActive")]
        public bool IsActive { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.Logo")]
        public string LogoUrl { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.SupportCapture")]
        public bool SupportCapture { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.SupportPartiallyRefund")]
        public bool SupportPartiallyRefund { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.SupportRefund")]
        public bool SupportRefund { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.SupportVoid")]
        public bool SupportVoid { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.RecurringPaymentType")]
        public string RecurringPaymentType { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.Configure")]
        public string ConfigurationUrl { get; set; }
    }
}