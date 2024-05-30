using PajoPhone.Models;
namespace PajoPhone;

public interface IProductBuilder
{
    IProductBuilder SetName(string name);
    IProductBuilder SetCategoryId(int categoryId);
    IProductBuilder SetColor(string color);
    IProductBuilder AddField(ICollection<Fields> field);
    IProductBuilder SetImage(IFormFile imageFile);
    Product Build(ProductViewModel viewModel);
}