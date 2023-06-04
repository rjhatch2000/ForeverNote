using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Orders
{
    public partial class ReturnReqestListModel : BaseForeverNoteModel
    {
        public ReturnReqestListModel()
        {
            ReturnRequestStatus = new List<SelectListItem>();
        }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.List.SearchCustomerEmail")]
        public string SearchCustomerEmail { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.List.SearchReturnRequestStatus")]
        public int SearchReturnRequestStatusId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.List.GoDirectlyToId")]
        public string GoDirectlyToId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.List.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [ForeverNoteResourceDisplayName("Admin.ReturnRequests.List.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        public string StoreId { get; set; }

        public IList<SelectListItem> ReturnRequestStatus { get; set; }
    }
}