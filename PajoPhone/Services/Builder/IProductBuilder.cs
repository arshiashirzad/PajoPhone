using Microsoft.EntityFrameworkCore;
using PajoPhone.Models;
namespace PajoPhone;

public abstract class IProductBuilder

{
    protected readonly Product _product = new Product() ;
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
    public IProductBuilder SetDescription(string description)
    {
        _product.Description = description;
        return this;
    }
    public abstract  IProductBuilder SetImage(IFormFile imageFile);

    public Product Build(ProductViewModel viewModel)
    {
        SetName(viewModel.Name);
        SetColor(viewModel.Color);
        SetDescription(viewModel.Description);
        SetCategoryId(viewModel.CategoryId);
        AddField(viewModel.Fields);
        SetImage(viewModel.ImageFile);
        return _product;
    }
}