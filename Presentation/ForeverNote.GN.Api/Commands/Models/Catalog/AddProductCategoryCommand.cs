﻿using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class AddProductCategoryCommand : IRequest<bool>
    {
        public ProductDto Product { get; set; }
        public ProductCategoryDto Model { get; set; }
    }
}