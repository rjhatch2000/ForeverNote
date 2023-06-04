﻿using ForeverNote.Api.Extensions;
using ForeverNote.Services.Catalog;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class UpdateProductTierPriceCommandHandler : IRequestHandler<UpdateProductTierPriceCommand, bool>
    {
        private readonly IProductService _productService;

        public UpdateProductTierPriceCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<bool> Handle(UpdateProductTierPriceCommand request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductById(request.Product.Id, true);
            var tierPrice = request.Model.ToEntity();
            tierPrice.ProductId = product.Id;
            await _productService.UpdateTierPrice(tierPrice);

            return true;
        }
    }
}
