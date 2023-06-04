using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mapping;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Documents;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Documents
{
    [Validator(typeof(DocumentValidator))]
    public class DocumentModel : BaseForeverNoteEntityModel, IAclMappingModel, IStoreMappingModel
    {
        public DocumentModel()
        {
            AvailableDocumentTypes = new List<SelectListItem>();
        }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.Number")]
        public string Number { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.Name")]
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.Description")]
        public string Description { get; set; }

        public string ParentDocumentId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.Picture")]
        [UIHint("Picture")]
        public string PictureId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.Download")]
        [UIHint("Download")]
        public string DownloadId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.Published")]
        public bool Published { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.Flag")]
        public string Flag { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.Link")]
        public string Link { get; set; }

        public string CustomerId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.Status")]
        public int StatusId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.Reference")]
        public int ReferenceId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.Object")]
        public string ObjectId { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.DocumentType")]
        public string DocumentTypeId { get; set; }
        public IList<SelectListItem> AvailableDocumentTypes { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.CustomerEmail")]
        public string CustomerEmail { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.Username")]
        public string Username { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.CurrencyCode")]
        public string CurrencyCode { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.TotalAmount")]
        public decimal TotalAmount { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.OutstandAmount")]
        public decimal OutstandAmount { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.Quantity")]
        public int Quantity { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.DocDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? DocDate { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.DueDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? DueDate { get; set; }

        //ACL
        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.SubjectToAcl")]
        public bool SubjectToAcl { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.AclCustomerRoles")]
        public List<CustomerRoleModel> AvailableCustomerRoles { get; set; }
        public string[] SelectedCustomerRoleIds { get; set; }

        //Store mapping
        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Documents.Document.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }
    }
}
