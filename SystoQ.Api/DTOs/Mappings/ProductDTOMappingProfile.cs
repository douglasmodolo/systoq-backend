using AutoMapper;
using SystoQ.Api.DTOs.Products;
using SystoQ.Domain.Entities;

namespace SystoQ.Api.DTOs.Mappings
{
    public class ProductDTOMappingProfile : Profile
    {
        public ProductDTOMappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductRequestDto>().ReverseMap();
            CreateMap<Product, UpdateProductResponseDto>().ReverseMap();
        }
    }
}
