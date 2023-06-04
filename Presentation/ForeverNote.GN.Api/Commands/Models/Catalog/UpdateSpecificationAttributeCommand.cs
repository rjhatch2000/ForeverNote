﻿using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class UpdateSpecificationAttributeCommand : IRequest<SpecificationAttributeDto>
    {
        public SpecificationAttributeDto Model { get; set; }
    }
}
