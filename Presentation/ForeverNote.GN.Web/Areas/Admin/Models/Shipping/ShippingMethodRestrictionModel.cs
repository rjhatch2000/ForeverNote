using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Areas.Admin.Models.Directory;
using System.Collections.Generic;

namespace ForeverNote.Web.Areas.Admin.Models.Shipping
{
    public partial class ShippingMethodRestrictionModel : BaseForeverNoteModel
    {
        public ShippingMethodRestrictionModel()
        {
            AvailableShippingMethods = new List<ShippingMethodModel>();
            AvailableCountries = new List<CountryModel>();
            AvailableCustomerRoles = new List<CustomerRoleModel>();
            Restricted = new Dictionary<string, IDictionary<string, bool>>();
            RestictedRole = new Dictionary<string, IDictionary<string, bool>>();
        }
        public IList<ShippingMethodModel> AvailableShippingMethods { get; set; }
        public IList<CountryModel> AvailableCountries { get; set; }
        public IList<CustomerRoleModel> AvailableCustomerRoles { get; set; }

        //[country id] / [shipping method id] / [restricted]
        public IDictionary<string, IDictionary<string, bool>> Restricted { get; set; }
        public IDictionary<string, IDictionary<string, bool>> RestictedRole { get; set; }
    }
}