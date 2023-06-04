using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForeverNote.Web.Areas.Admin.Models.Customers
{
    public partial class CustomerListModel : BaseForeverNoteModel
    {
        public CustomerListModel()
        {
            AvailableCustomerTags = new List<SelectListItem>();
            SearchCustomerTagIds = new List<string>();
            SearchCustomerRoleIds = new List<string>();
            AvailableCustomerRoles = new List<SelectListItem>();
        }

        [ForeverNoteResourceDisplayName("Admin.Customers.Customers.List.CustomerRoles")]
        
        public IList<SelectListItem> AvailableCustomerRoles { get; set; }


        [ForeverNoteResourceDisplayName("Admin.Customers.Customers.List.CustomerRoles")]
        [UIHint("MultiSelect")]
        public IList<string> SearchCustomerRoleIds { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Customers.Customers.List.CustomerTags")]
        public IList<SelectListItem> AvailableCustomerTags { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Customers.Customers.List.CustomerTags")]
        [UIHint("MultiSelect")]
        public IList<string> SearchCustomerTagIds { get; set; }


        [ForeverNoteResourceDisplayName("Admin.Customers.Customers.List.SearchEmail")]
        
        public string SearchEmail { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Customers.Customers.List.SearchUsername")]
        
        public string SearchUsername { get; set; }
        public bool UsernamesEnabled { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Customers.Customers.List.SearchFirstName")]
        
        public string SearchFirstName { get; set; }
        [ForeverNoteResourceDisplayName("Admin.Customers.Customers.List.SearchLastName")]
        
        public string SearchLastName { get; set; }


        [ForeverNoteResourceDisplayName("Admin.Customers.Customers.List.SearchCompany")]
        
        public string SearchCompany { get; set; }
        public bool CompanyEnabled { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Customers.Customers.List.SearchPhone")]
        
        public string SearchPhone { get; set; }
        public bool PhoneEnabled { get; set; }

        [ForeverNoteResourceDisplayName("Admin.Customers.Customers.List.SearchZipCode")]
        
        public string SearchZipPostalCode { get; set; }
        public bool ZipPostalCodeEnabled { get; set; }
    }
}