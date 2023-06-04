﻿using ForeverNote.Services.Catalog;
using ForeverNote.Services.Localization;
using ForeverNote.Web.Features.Models.Catalog;
using ForeverNote.Web.Models.Catalog;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Web.Features.Handlers.Catalog
{
    public class GetProductTagsAllHandler : IRequestHandler<GetProductTagsAll, PopularProductTagsModel>
    {
        private readonly IProductTagService _productTagService;

        public GetProductTagsAllHandler(IProductTagService productTagService)
        {
            _productTagService = productTagService;
        }

        public async Task<PopularProductTagsModel> Handle(GetProductTagsAll request, CancellationToken cancellationToken)
        {
            var model = new PopularProductTagsModel();
            var tagsTask = (await _productTagService
                .GetAllProductTags())
                .OrderBy(x => x.Name)
                .Select(async x =>
                {
                    var ptModel = new ProductTagModel {
                        Id = x.Id,
                        Name = x.GetLocalized(y => y.Name, request.Language.Id),
                        SeName = x.SeName,
                        ProductCount = await _productTagService.GetProductCount(x.Id, request.Store.Id)
                    };
                    return ptModel;
                });

            model.Tags = await Task.WhenAll(tagsTask);
            return model;
        }
    }
}
