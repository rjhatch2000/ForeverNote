using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Stores;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ForeverNote.Web.Commands.Models.ShoppingCart
{
    public class SaveCheckoutAttributesCommand : IRequest<string>
    {
        public Customer Customer { get; set; }
        public Store Store { get; set; }

        public IList<ShoppingCartItem> Cart { get; set; }
        public IFormCollection Form { get; set; }
    }
}
