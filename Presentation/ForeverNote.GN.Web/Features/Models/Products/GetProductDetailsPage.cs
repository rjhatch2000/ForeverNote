using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Core.Domain.Stores;
using ForeverNote.Web.Models.Catalog;
using MediatR;

namespace ForeverNote.Web.Features.Models.Products
{
    public class GetProductDetailsPage : IRequest<ProductDetailsModel>
    {
        public Store Store { get; set; }
        public Product Product { get; set; }
        public ShoppingCartItem UpdateCartItem { get; set; } = null;
        public bool IsAssociatedProduct { get; set; } = false;
    }
}
