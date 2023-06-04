using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Catalog;
using MediatR;

namespace ForeverNote.Web.Features.Models.Catalog
{
    public class GetManufacturerNavigation : IRequest<ManufacturerNavigationModel>
    {
        public string CurrentManufacturerId { get; set; }
        public Customer Customer { get; set; }
        public Store Store { get; set; }
        public Language Language { get; set; }
    }
}
