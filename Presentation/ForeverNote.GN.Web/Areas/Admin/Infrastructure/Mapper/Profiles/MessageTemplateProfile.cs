﻿using AutoMapper;
using ForeverNote.Core.Domain.Messages;
using ForeverNote.Core.Infrastructure.Mapper;
using ForeverNote.Web.Areas.Admin.Extensions;
using ForeverNote.Web.Areas.Admin.Models.Messages;
using System.Collections.Generic;
using System.Linq;

namespace ForeverNote.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class MessageTemplateProfile : Profile, IMapperProfile
    {
        public MessageTemplateProfile()
        {
            CreateMap<MessageTemplate, MessageTemplateModel>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore())
                .ForMember(dest => dest.AllowedTokens, mo => mo.Ignore())
                .ForMember(dest => dest.HasAttachedDownload, mo => mo.Ignore())
                .ForMember(dest => dest.Locales, mo => mo.Ignore())
                .ForMember(dest => dest.AvailableEmailAccounts, mo => mo.Ignore())
                .ForMember(dest => dest.AvailableStores, mo => mo.Ignore())
                .ForMember(dest => dest.ListOfStores, mo => mo.Ignore())
                .ForMember(dest => dest.SelectedStoreIds, mo => mo.Ignore());

            CreateMap<MessageTemplateModel, MessageTemplate>()
                .ForMember(dest => dest.Id, mo => mo.Ignore())
                .ForMember(dest => dest.DelayPeriod, mo => mo.Ignore())
                .ForMember(dest => dest.Stores, mo => mo.MapFrom(x => x.SelectedStoreIds != null ? x.SelectedStoreIds.ToList() : new List<string>()))
                .ForMember(dest => dest.Locales, mo => mo.MapFrom(x => x.Locales.ToLocalizedProperty()));
        }

        public int Order => 0;
    }
}