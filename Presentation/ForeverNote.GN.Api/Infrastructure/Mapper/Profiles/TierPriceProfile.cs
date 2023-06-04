using AutoMapper;
using ForeverNote.Api.DTOs.Catalog;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Infrastructure.Mapper;

namespace ForeverNote.Api.Infrastructure.Mapper
{
    public class TierPriceProfile : Profile, IMapperProfile
    {
        public TierPriceProfile()
        {
            CreateMap<ProductTierPriceDto, TierPrice>();

            CreateMap<TierPrice, ProductTierPriceDto>();
        }

        public int Order => 1;
    }
}