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
                    UpdateExistingFieldValues(dest, src);
                    AddNewFieldValues(dest, src);
                    MarkDeletedFieldValues(dest, src);
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
        private void UpdateExistingFieldValues(Product dest, ProductViewModel src)
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
            }
        }
        private void AddNewFieldValues(Product dest, ProductViewModel src)
        {
            foreach (var fv in src.FieldsValues)
            {
                var currentFieldValue = dest.FieldsValues
                    .FirstOrDefault(f => f.FieldKeyId == fv.KeyId);
                if (currentFieldValue == null)
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
        }
        private void MarkDeletedFieldValues(Product dest, ProductViewModel src)
        {
            foreach (var fieldValue in dest.FieldsValues)
            {
                if (src.FieldsValues.All(f => f.ValueId != fieldValue.Id))
                {
                    fieldValue.DeletedAt = DateTime.Now;
                }
            }
        }
}
}