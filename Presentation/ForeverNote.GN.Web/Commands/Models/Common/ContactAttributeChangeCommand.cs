using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Stores;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ForeverNote.Web.Commands.Models.Common
{
    public class ContactAttributeChangeCommand : IRequest<(IList<string> enabledAttributeIds, IList<string> disabledAttributeIds)>
    {
        public IFormCollection Form { get; set; }
        public Customer Customer { get; set; }
        public Store Store { get; set; }

    }
}
