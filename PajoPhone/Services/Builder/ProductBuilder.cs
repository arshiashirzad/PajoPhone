using PajoPhone.Models;

namespace PajoPhone;

public class ProductBuilder : IProductBuilder
{
      public override IProductBuilder SetImage(IFormFile imageFile)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
               imageFile.CopyTo(memoryStream);
               _product.Image = memoryStream.ToArray();
            }
        }
        return this;
    }
}