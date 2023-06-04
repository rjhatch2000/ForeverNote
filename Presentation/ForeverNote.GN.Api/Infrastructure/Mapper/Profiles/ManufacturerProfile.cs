﻿using AutoMapper;
using ForeverNote.Api.DTOs.Catalog;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Infrastructure.Mapper;

namespace ForeverNote.Api.Infrastructure.Mapper
{
    public class ManufacturerProfile : Profile, IMapperProfile
    {
        public ManufacturerProfile()
        {

            CreateMap<ManufacturerDto, Manufacturer>()
                .ForMember(dest => dest.SubjectToAcl, mo => mo.Ignore())
                .ForMember(dest => dest.CustomerRoles, mo => mo.Ignore())
                .ForMember(dest => dest.LimitedToStores, mo => mo.Ignore())
                .ForMember(dest => dest.Stores, mo => mo.Ignore())
                .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore())
                .ForMember(dest => dest.UpdatedOnUtc, mo => mo.Ignore())
                .ForMember(dest => dest.Locales, mo => mo.Ignore())
                .ForMember(dest => dest.AppliedDiscounts, mo => mo.Ignore())
                .ForMember(dest => dest.GenericAttributes, mo => mo.Ignore());

            CreateMap<Manufacturer, ManufacturerDto>();

        }

        public int Order => 1;
    }
}