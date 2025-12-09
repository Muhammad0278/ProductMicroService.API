

using AutoMapper;
using eCommerce.BusinessLogicLayer.DTO;
using eCommerce.DataAccessLayer.Entities;

namespace eCommerce.BusinessLogicLayer.Mappers;

public class ProductAddRequestMappingProfile : Profile

{

    public ProductAddRequestMappingProfile()
    {
        CreateMap<ProductAddRequest, Product>()
            .ForMember(
                dest => dest.ProductID,
                opt => opt.MapFrom(src => Guid.NewGuid())
            )
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.ProductName)
            )
            .ForMember(
                dest => dest.Category,
                opt => opt.MapFrom(src => src.Category)
            )
            .ForMember(
                dest => dest.UnitPrice,
                opt => opt.MapFrom(src => src.UnitPrice)
            )
            .ForMember(
                dest => dest.QuantityInStock,
                opt => opt.MapFrom(src => src.QuantityInStock)
            );
    }
}
