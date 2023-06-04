using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Validators.Customers;

namespace ForeverNote.Web.Areas.Admin.Models.Customers
{
    [Validator(typeof(CustomerTagValidator))]
    public partial class CustomerTagModel : BaseForeverNoteEntityModel
    {
        [ForeverNoteResourceDisplayName("Admin.Customers.CustomerTags.Fields.Name")]
        
        public string Name { get; set; }
    }
}