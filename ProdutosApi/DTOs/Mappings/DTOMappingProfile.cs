using AutoMapper;
using ProdutosApi.Models;


namespace ProdutosApi.DTOs.Mappings
{
    public class DTOMappingProfile : Profile
    {
        public DTOMappingProfile() 
        {
            CreateMap<Product, ProductInputDto>().ReverseMap();
            CreateMap<Product, ProductViewDto>().ReverseMap();
            CreateMap<Category, CategoryInputDto>().ReverseMap();
            CreateMap<Category, CategoryViewDto>().ReverseMap();
        }
    }
}
