using PajoPhone.Models;

namespace PajoPhone;

public class ProductBuilder: IProductBuilder
{
    private readonly Product _product;
    private readonly ApplicationDbContext _context;

    public ProductBuilder(ApplicationDbContext context)
    {
        _product = new Product();
        _context = context;
    }

    public IProductBuilder SetName(string name)
    {
        _product.Name = name;
        return this;
    }

    public IProductBuilder SetCategoryId(int categoryId)
    {
        _product.CategoryId = categoryId;
        return this;
    }

    public IProductBuilder AddField(Fields field)
    {
        _product.Fields.Add(field);
        return this;
    }

    public IProductBuilder SetImage(IFormFile imageFile)
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

    public Product Build()
    {
        return _product;
    }
}