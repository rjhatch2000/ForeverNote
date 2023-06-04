﻿using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Web.Models.Catalog;
using MediatR;
using System.Collections.Generic;

namespace ForeverNote.Web.Features.Models.Products
{
    public class GetProductOverview : IRequest<IEnumerable<ProductOverviewModel>>
    {
        public IEnumerable<Product> Products { get; set; }
        public bool PreparePriceModel { get; set; } = true;
        public bool PreparePictureModel { get; set; } = true;
        public int? ProductThumbPictureSize { get; set; } = null;
        public bool PrepareSpecificationAttributes { get; set; } = false;
        public bool ForceRedirectionAfterAddingToCart { get; set; } = false;
    }
}
