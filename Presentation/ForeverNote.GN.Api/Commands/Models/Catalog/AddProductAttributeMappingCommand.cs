﻿using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class AddProductAttributeMappingCommand : IRequest<ProductAttributeMappingDto>
    {
        public ProductDto Product { get; set; }
        public ProductAttributeMappingDto Model { get; set; }
    }
}
