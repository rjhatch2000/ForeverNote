using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Affiliates
{
    public partial class AffiliateListModel : BaseForeverNoteModel
    {
        [ForeverNoteResourceDisplayName("Admin.Affiliates.List.SearchFirstName")]
        
        public string SearchFirstName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Affiliates.List.SearchLastName")]
        
        public string SearchLastName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Affiliates.List.SearchFriendlyUrlName")]
        
        public string SearchFriendlyUrlName { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Affiliates.List.LoadOnlyWithOrders")]
        public bool LoadOnlyWithOrders { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Affiliates.List.OrdersCreatedFromUtc")]
        [UIHint("DateNullable")]
        public DateTime? OrdersCreatedFromUtc { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Affiliates.List.OrdersCreatedToUtc")]
        [UIHint("DateNullable")]
        public DateTime? OrdersCreatedToUtc { get; set; }
    }
}