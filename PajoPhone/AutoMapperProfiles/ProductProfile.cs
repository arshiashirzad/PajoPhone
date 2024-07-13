using AutoMapper;
using PajoPhone.Models;
namespace PajoPhone.AutoMapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductViewModel>();

            CreateMap<ProductViewModel, Product>()
                .ForMember(dest => dest.FieldsValues, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    foreach (var fv in src.FieldsValues)
                    {
                        var currentFieldValue = dest.FieldsValues
                            .FirstOrDefault(f => f.FieldKeyId == fv.KeyId);
                        if (currentFieldValue != null)
                        {
                            currentFieldValue.StringValue = fv.StringValue!;
                            currentFieldValue.IntValue = fv.IntValue;
                            currentFieldValue.DeletedAt = null;
                        }
                        else
                        {
                            dest.FieldsValues.Add(new FieldsValue
                            {
                                FieldKeyId = fv.KeyId,
                                StringValue = fv.StringValue!,
                                IntValue = fv.IntValue,
                                DeletedAt = null
                            });
                        }
                    }
                    foreach (var fieldValue in dest.FieldsValues)
                    {
                        if (src.FieldsValues.All(f => f.ValueId != fieldValue.Id))
                        {
                            fieldValue.DeletedAt = DateTime.Now;
                        }
                    }
                })
                .ForMember(opt => opt.Image,
                    dest => dest.MapFrom(x => GetByteArray(x.ImageFile!)));
        }

        public byte[] GetByteArray(IFormFile iformfile)
        {
            var ms = new MemoryStream();
            iformfile.CopyTo(ms);
            return ms.ToArray();
        }
}
}