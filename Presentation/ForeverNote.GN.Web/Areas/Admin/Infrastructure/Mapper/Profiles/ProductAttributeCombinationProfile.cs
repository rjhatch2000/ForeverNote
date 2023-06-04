using AutoMapper;
using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Infrastructure.Mapper;
using ForeverNote.Web.Areas.Admin.Models.Catalog;

namespace ForeverNote.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class ProductAttributeCombinationProfile : Profile, IMapperProfile
    {
        public ProductAttributeCombinationProfile()
        {
            CreateMap<ProductAttributeCombination, ProductAttributeCombinationModel>()
                .ForMember(dest => dest.UseMultipleWarehouses, mo => mo.Ignore())
                .ForMember(dest => dest.WarehouseInventoryModels, mo => mo.Ignore());
            CreateMap<ProductAttributeCombinationModel, ProductAttributeCombination>()
                .ForMember(dest => dest.WarehouseInventory, mo => mo.Ignore())
                .ForMember(dest => dest.Id, mo => mo.Ignore());

            CreateMap<PredefinedProductAttributeValue, ProductAttributeValue>()
               .ForMember(dest => dest.AttributeValueType, mo => mo.MapFrom(x => AttributeValueType.Simple))
               .ForMember(dest => dest.ProductAttributeMappingId, mo => mo.MapFrom(x => x.Id))
               .ForMember(dest => dest.Id, mo => mo.Ignore());
        }

        public int Order => 0;
    }
}