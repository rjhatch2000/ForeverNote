using AutoMapper;
using ForeverNote.Api.DTOs.Customers;
using ForeverNote.Core.Domain.Common;
using ForeverNote.Core.Infrastructure.Mapper;

namespace ForeverNote.Api.Infrastructure.Mapper
{
    public class AddressProfile : Profile, IMapperProfile
    {
        public AddressProfile()
        {
            CreateMap<AddressDto, Address>()
                .ForMember(dest => dest.CustomAttributes, mo => mo.Ignore())
                .ForMember(dest => dest.GenericAttributes, mo => mo.Ignore());

            CreateMap<Address, AddressDto>();
        }

        public int Order => 1;
    }
}
