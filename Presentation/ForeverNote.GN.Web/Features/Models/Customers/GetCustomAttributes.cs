using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Web.Models.Customer;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Web.Features.Models.Customers
{
    public class GetCustomAttributes : IRequest<IList<CustomerAttributeModel>>
    {
        public Customer Customer { get; set; }
        public Language Language { get; set; }
        public string OverrideAttributesXml { get; set; } = "";
    }
}
