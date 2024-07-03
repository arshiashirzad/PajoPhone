using AutoMapper;
using PajoPhone.Models;
namespace PajoPhone.AutoMapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.ImageFile,
                    opt =>
                        opt.MapFrom(src => src.Image));
            CreateMap<ProductViewModel, Product>()
                .ForMember(opt => opt.Image,
                    dest => dest.MapFrom(x => GetByteArray(x.ImageFile)))
                .ForMember(opt => opt.FieldsValues, src => src.MapFrom(x => x.FieldsValues.Select(f =>
                     new FieldsValue()
                    {
                        FieldKeyId = f.Id,
                        StringValue = f.StringValue,
                        IntValue =f.IntValue
                    })));
        }

        public byte[] GetByteArray(IFormFile iformfile)
        {
            var ms = new MemoryStream();
            iformfile.CopyTo(ms);
            return ms.ToArray();
        }
}
}