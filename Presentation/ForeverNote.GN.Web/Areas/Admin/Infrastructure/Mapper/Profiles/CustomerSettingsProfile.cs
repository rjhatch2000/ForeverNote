﻿using AutoMapper;
using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Infrastructure.Mapper;
using ForeverNote.Web.Areas.Admin.Models.Settings;

namespace ForeverNote.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class CustomerSettingsProfile : Profile, IMapperProfile
    {
        public CustomerSettingsProfile()
        {
            CreateMap<CustomerSettings, CustomerUserSettingsModel.CustomerSettingsModel>()
                .ForMember(dest => dest.GenericAttributes, mo => mo.Ignore());
            CreateMap<CustomerUserSettingsModel.CustomerSettingsModel, CustomerSettings>()
                .ForMember(dest => dest.HashedPasswordFormat, mo => mo.Ignore())
                .ForMember(dest => dest.AvatarMaximumSizeBytes, mo => mo.Ignore())
                .ForMember(dest => dest.DownloadableProductsValidateUser, mo => mo.Ignore())
                .ForMember(dest => dest.OnlineCustomerMinutes, mo => mo.Ignore())
                .ForMember(dest => dest.SuffixDeletedCustomers, mo => mo.Ignore());
        }

        public int Order => 0;
    }
}