using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Models.Common;
using ForeverNote.Web.Areas.Admin.Validators.Shipping;

namespace ForeverNote.Web.Areas.Admin.Models.Shipping
{
    [Validator(typeof(WarehouseValidator))]
    public partial class WarehouseModel : BaseForeverNoteEntityModel
    {
        public WarehouseModel()
        {
            this.Address = new AddressModel();
        }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Shipping.Warehouses.Fields.Name")]
        
        public string Name { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Shipping.Warehouses.Fields.AdminComment")]
        
        public string AdminComment { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Configuration.Shipping.Warehouses.Fields.Address")]
        public AddressModel Address { get; set; }
    }
}