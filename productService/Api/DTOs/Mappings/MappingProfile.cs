using AutoMapper;
using productService.Domain.Entities;

namespace productService.Api.DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Category, CategoriesWithProductsDTO>().ReverseMap();

        CreateMap<Product, ProductDTO>().ReverseMap();

        CreateMap<Product, ProductDTO>()
            .ForMember(pdto => pdto.CategoryName, 
            opts => opts.MapFrom(src => src.Category.Name));  
    }
}