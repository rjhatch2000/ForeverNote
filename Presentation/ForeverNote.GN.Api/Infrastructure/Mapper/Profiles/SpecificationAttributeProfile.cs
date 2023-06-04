﻿using AutoMapper;
using ForeverNote.Api.DTOs.Catalog;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Infrastructure.Mapper;

namespace ForeverNote.Api.Infrastructure.Mapper
{
    public class SpecificationAttributeProfile : Profile, IMapperProfile
    {
        public SpecificationAttributeProfile()
        {

            CreateMap<SpecificationAttributeDto, SpecificationAttribute>()
                .ForMember(dest => dest.GenericAttributes, mo => mo.Ignore());

            CreateMap<SpecificationAttribute, SpecificationAttributeDto>();

            CreateMap<SpecificationAttributeOption, SpecificationAttributeOptionDto>();

            CreateMap<SpecificationAttributeOptionDto, SpecificationAttributeOption>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore());

        }

        public int Order => 1;
    }
}
