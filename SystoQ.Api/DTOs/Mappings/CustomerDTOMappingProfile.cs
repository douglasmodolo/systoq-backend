using AutoMapper;
using SystoQ.Api.DTOs.Customers;
using SystoQ.Domain.Entities;

namespace SystoQ.Api.DTOs.Mappings
{
    public class CustomerDTOMappingProfile : Profile
    {
        public CustomerDTOMappingProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Customer, UpdateCustomerRequestDto>().ReverseMap();
            CreateMap<Customer, UpdateCustomerResponseDto>().ReverseMap();
        }

    }
}
