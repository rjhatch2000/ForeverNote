﻿using ForeverNote.Api.Commands.Models.Catalog;
using ForeverNote.Services.Catalog;
using ForeverNote.Services.Localization;
using ForeverNote.Services.Logging;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Api.Commands.Handlers.Catalog
{
    public class DeleteProductAttributeCommandHandler : IRequestHandler<DeleteProductAttributeCommand, bool>
    {
        private readonly IProductAttributeService _productAttributeService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;

        public DeleteProductAttributeCommandHandler(
            IProductAttributeService productAttributeService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService)
        {
            _productAttributeService = productAttributeService;
            _customerActivityService = customerActivityService;
            _localizationService = localizationService;
        }

        public async Task<bool> Handle(DeleteProductAttributeCommand request, CancellationToken cancellationToken)
        {
            var productAttribute = await _productAttributeService.GetProductAttributeById(request.Model.Id);
            if (productAttribute != null)
            {
                await _productAttributeService.DeleteProductAttribute(productAttribute);
                //activity log
                await _customerActivityService.InsertActivity("DeleteProductAttribute", productAttribute.Id,
                    _localizationService.GetResource("ActivityLog.DeleteProductAttribute"), productAttribute.Name);
            }
            return true;
        }
    }
}
