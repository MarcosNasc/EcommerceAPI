using AutoMapper;
using Ecommerce.API.DTOs;
using Ecommerce.BLL.Entities;

namespace Ecommerce.API.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            CreateMap<Supplier, SupplierDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
