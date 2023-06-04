using FluentValidation.Attributes;
using ForeverNote.Web.Framework.Mvc.ModelBinding;
using ForeverNote.Web.Framework.Mvc.Models;
using ForeverNote.Web.Validators.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ForeverNote.Web.Models.Vendors
{
    [Validator(typeof(VendorAddressValidator))]
    public partial class VendorAddressModel : BaseForeverNoteEntityModel
    {
        public VendorAddressModel()
        {
            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
        }

        public bool CompanyEnabled { get; set; }
        public bool CompanyRequired { get; set; }
        [ForeverNoteResourceDisplayName("Account.VendorInfo.Company")]
        public string Company { get; set; }
        public bool CountryEnabled { get; set; }
        [ForeverNoteResourceDisplayName("Account.VendorInfo.Country")]
        public string CountryId { get; set; }
        [ForeverNoteResourceDisplayName("Account.VendorInfo.Country")]
        public string CountryName { get; set; }
        public bool StateProvinceEnabled { get; set; }
        [ForeverNoteResourceDisplayName("Account.VendorInfo.StateProvince")]
        public string StateProvinceId { get; set; }
        [ForeverNoteResourceDisplayName("Account.VendorInfo.StateProvince")]
        public string StateProvinceName { get; set; }
        public bool CityEnabled { get; set; }
        public bool CityRequired { get; set; }
        [ForeverNoteResourceDisplayName("Account.VendorInfo.City")]
        public string City { get; set; }
        public bool StreetAddressEnabled { get; set; }
        public bool StreetAddressRequired { get; set; }
        [ForeverNoteResourceDisplayName("Account.VendorInfo.Address1")]
        public string Address1 { get; set; }
        public bool StreetAddress2Enabled { get; set; }
        public bool StreetAddress2Required { get; set; }
        [ForeverNoteResourceDisplayName("Account.VendorInfo.Address2")]
        public string Address2 { get; set; }
        public bool ZipPostalCodeEnabled { get; set; }
        public bool ZipPostalCodeRequired { get; set; }
        [ForeverNoteResourceDisplayName("Account.VendorInfo.ZipPostalCode")]
        public string ZipPostalCode { get; set; }
        public bool PhoneEnabled { get; set; }
        public bool PhoneRequired { get; set; }
        [ForeverNoteResourceDisplayName("Account.VendorInfo.PhoneNumber")]
        public string PhoneNumber { get; set; }
        public bool FaxEnabled { get; set; }
        public bool FaxRequired { get; set; }
        [ForeverNoteResourceDisplayName("Account.VendorInfo.FaxNumber")]
        public string FaxNumber { get; set; }
        public IList<SelectListItem> AvailableCountries { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }
    }
}