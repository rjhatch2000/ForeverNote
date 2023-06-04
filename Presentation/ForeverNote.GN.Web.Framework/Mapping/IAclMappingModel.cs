using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using System.Collections.Generic;

namespace ForeverNote.Web.Framework.Mapping
{
    public interface IAclMappingModel
    {
        [ForeverNoteResourceDisplayName("Admin.Catalog.Categories.Fields.SubjectToAcl")]
        bool SubjectToAcl { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Catalog.Categories.Fields.AclCustomerRoles")]
        List<CustomerRoleModel> AvailableCustomerRoles { get; set; }
        string[] SelectedCustomerRoleIds { get; set; }
    }
}
