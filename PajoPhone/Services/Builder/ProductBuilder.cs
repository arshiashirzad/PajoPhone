using PajoPhone.Models;

namespace PajoPhone;

public class ProductBuilder: IProductBuilder
{
    private readonly Product _product;

    public ProductBuilder( )
    {
        _product = new Product();
    }

    public IProductBuilder SetName(string name)
    {
        return this;
    }

    public IProductBuilder SetCategoryId(int categoryId)
    {
        _product.CategoryId = categoryId;
        return this;
    }

    public IProductBuilder AddField(ICollection<Fields> field)
    {
        _product.Fields = field;
        return this;
    }

    public IProductBuilder SetColor(string color)
    {
        _product.Color = color;
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

    public Product Build(ProductViewModel viewModel)
    {
        SetName(viewModel.Name);
        SetColor(viewModel.Color);
        SetImage(viewModel.ImageFile);
        SetCategoryId(viewModel.CategoryId);
        AddField(viewModel.Fields);
        return _product;
    }
}