using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Orders
{
    public partial class ReturnRequestModel : BaseForeverNoteEntityModel
    {
        public ReturnRequestModel()
        {
            Items = new List<ReturnRequestItemModel>();
        }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.Fields.ID")]
        public override string Id { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.Fields.ID")]
        public int ReturnNumber { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.Fields.Order")]
        public string OrderId { get; set; }
        public int OrderNumber { get; set; }
        public string OrderCode { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.Fields.ExternalId")]
        public string ExternalId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.Fields.Customer")]
        public string CustomerId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.Fields.Customer")]
        public string CustomerInfo { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.Fields.Total")]
        public string Total { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.Fields.CustomerComments")]
        public string CustomerComments { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.Fields.StaffNotes")]
        public string StaffNotes { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.Fields.Status")]
        public int ReturnRequestStatusId { get; set; }
        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.Fields.Status")]
        public string ReturnRequestStatusStr { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.Fields.Quantity")]
        public int Quantity { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.Fields.PickupDate")]
        public DateTime PickupDate { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.Fields.PickupAddress")]
        public AddressModel PickupAddress { get; set; }

        public List<ReturnRequestItemModel> Items { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.NotifyCustomer")]
        public bool NotifyCustomer { get; set; }

        //return request notes
        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.ReturnRequestNotes.Fields.DisplayToCustomer")]
        public bool AddReturnRequestNoteDisplayToCustomer { get; set; }
        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.ReturnRequestNotes.Fields.Note")]
        public string AddReturnRequestNoteMessage { get; set; }
        public bool AddReturnRequestNoteHasDownload { get; set; }
        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.ReturnRequestNotes.Fields.Download")]
        [UIHint("Download")]
        public string AddReturnRequestNoteDownloadId { get; set; }

        public class ReturnRequestItemModel : BaseForeverNoteEntityModel
        {
            public string ProductId { get; set; }

            public string ProductName { get; set; }

            public string UnitPrice { get; set; }

            public int Quantity { get; set; }

            public string ReasonForReturn { get; set; }

            public string RequestedAction { get; set; }
        }

        public partial class ReturnRequestNote : BaseForeverNoteEntityModel
        {
            public string ReturnRequestId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.ReturnRequests.ReturnRequestNotes.Fields.DisplayToCustomer")]
            public bool DisplayToCustomer { get; set; }
            [ForeverNoteResourceDisplayName("Admin.ReturnRequests.ReturnRequestNotes.Fields.Note")]
            public string Note { get; set; }
            [ForeverNoteResourceDisplayName("Admin.ReturnRequests.ReturnRequestNotes.Fields.Download")]
            public string DownloadId { get; set; }
            [ForeverNoteResourceDisplayName("Admin.ReturnRequests.ReturnRequestNotes.Fields.Download")]
            public Guid DownloadGuid { get; set; }
            [ForeverNoteResourceDisplayName("Admin.ReturnRequests.ReturnRequestNotes.Fields.CreatedOn")]
            public DateTime CreatedOn { get; set; }
            [ForeverNoteResourceDisplayName("Admin.ReturnRequests.ReturnRequestNotes.Fields.CreatedByCustomer")]
            public bool CreatedByCustomer { get; set; }
        }
    }
}