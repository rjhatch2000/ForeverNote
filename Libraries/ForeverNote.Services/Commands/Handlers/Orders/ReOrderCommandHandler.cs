using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Orders;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Commands.Models.Orders;
using ForeverNote.Services.Customers;
using ForeverNote.Services.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Commands.Handlers.Orders
{
    public class ReOrderCommandHandler : IRequestHandler<ReOrderCommand, IList<string>>
    {
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;

        public ReOrderCommandHandler(
            ICustomerService customerService, 
            IProductService productService, 
            IShoppingCartService shoppingCartService)
        {
            _customerService = customerService;
            _productService = productService;
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IList<string>> Handle(ReOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.Order == null)
                throw new ArgumentNullException("order");

            var warnings = new List<string>();
            var customer = await _customerService.GetCustomerById(request.Order.CustomerId);

            foreach (var orderItem in request.Order.OrderItems)
            {
                var product = await _productService.GetProductById(orderItem.ProductId);
                if (product != null)
                {
                    if (product.ProductType == ProductType.SimpleProduct)
                    {
                        warnings.AddRange(await _shoppingCartService.AddToCart(customer, orderItem.ProductId,
                            ShoppingCartType.ShoppingCart, request.Order.StoreId, orderItem.WarehouseId,
                            orderItem.AttributesXml, orderItem.UnitPriceExclTax,
                            orderItem.RentalStartDateUtc, orderItem.RentalEndDateUtc,
                            orderItem.Quantity, false));
                    }
                }
                else
                {
                    warnings.Add("Product is not available");
                }
            }

            return warnings;
        }
    }
}
