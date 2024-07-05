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
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    if (src.ImageFile != null)
                    {
                        dest.Image = GetByteArray(src.ImageFile);
                    }
                })
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