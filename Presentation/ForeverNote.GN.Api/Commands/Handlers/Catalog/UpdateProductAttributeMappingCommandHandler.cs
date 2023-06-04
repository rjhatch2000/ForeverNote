﻿using ForeverNote.Api.DTOs.Catalog;
using ForeverNote.Api.Extensions;
using ForeverNote.Services.Catalog;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class UpdateProductAttributeMappingCommandHandler : IRequestHandler<UpdateProductAttributeMappingCommand, ProductAttributeMappingDto>
    {
        private readonly IProductAttributeService _productAttributeService;

        public UpdateProductAttributeMappingCommandHandler(IProductAttributeService productAttributeService)
        {
            _productAttributeService = productAttributeService;
        }

        public async Task<ProductAttributeMappingDto> Handle(UpdateProductAttributeMappingCommand request, CancellationToken cancellationToken)
        {
            //insert mapping
            var productAttributeMapping = request.Model.ToEntity();
            productAttributeMapping.ProductId = request.Product.Id;
            await _productAttributeService.UpdateProductAttributeMapping(productAttributeMapping, true);

            return productAttributeMapping.ToModel();
        }
    }
}
