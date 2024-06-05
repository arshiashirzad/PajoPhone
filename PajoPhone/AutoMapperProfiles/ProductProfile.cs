using AutoMapper;
using PajoPhone.Models;
namespace PajoPhone.AutoMapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.ImageFile ,
                    opt=>
                        opt.MapFrom(src => src.Image));
            CreateMap<ProductViewModel, Product>()
                .ForMember(opt => opt.Image,
                    dest => dest.MapFrom(src =>
                        src.ImageFile));
        }
    }
}