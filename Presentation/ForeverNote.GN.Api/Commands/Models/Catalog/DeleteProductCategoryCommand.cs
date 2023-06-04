﻿using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class DeleteProductCategoryCommand : IRequest<bool>
    {
        public ProductDto Product { get; set; }
        public string CategoryId { get; set; }
    }
}