using AutoMapper;
using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            
            //CreateMap<Product, ProductDetailDto>()
            //.ForMember(dest => dest.UnitInStock, opt => opt.MapFrom(src => src.UnitsInStock));
            //CreateMap<Product, ProductDetailDto>()
            //.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName));

            //CreateMap<Product, ProductDetailDto>()
            //.ForMember(dest => dest.CategoryName,
                       //opt => opt.MapFrom(src => src.Category.CategoryName))
                       //.ForMember(dest => dest.UnitInStock, opt => opt.MapFrom(src => src.UnitsInStock));
        }
    }
}
