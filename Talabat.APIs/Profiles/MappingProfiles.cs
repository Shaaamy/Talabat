using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;

namespace Talabat.APIs.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product,ProductToReturnDto>()
                     .ForMember(d=>d.ProductType ,o=>o.MapFrom(s=>s.ProductType.Name))    
                     .ForMember(d=>d.ProductBrand ,o=>o.MapFrom(s=>s.ProductBrand.Name));    
        }
    }
}
