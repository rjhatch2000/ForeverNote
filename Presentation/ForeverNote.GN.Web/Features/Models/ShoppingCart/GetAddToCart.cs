using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Directory;
using ForeverNote.Core.Domain.Localization;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Core.Domain.Tax;
using ForeverNote.Web.Models.ShoppingCart;
using MediatR;
using System;

namespace ForeverNote.Web.Features.Models.ShoppingCart
{
    public class GetAddToCart : IRequest<AddToCartModel>
    {
        public Product Product { get; set; }
        public Customer Customer { get; set; }
        public Language Language { get; set; }
        public Currency Currency { get; set; }
        public Store Store { get; set; }
        public TaxDisplayType TaxDisplayType { get; set; }
        public int Quantity { get; set; }
        public decimal CustomerEnteredPrice { get; set; }
        public string AttributesXml { get; set; }
        public ShoppingCartType CartType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ReservationId { get; set; }
        public string Parameter { get; set; }
        public string Duration { get; set; }
    }
}
