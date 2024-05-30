using PajoPhone.Models;
namespace PajoPhone;

public interface IProductBuilder
{
    IProductBuilder SetName(string name);
    IProductBuilder SetCategoryId(int categoryId);
    IProductBuilder AddField(Fields field);
    IProductBuilder SetImage(IFormFile imageFile);
    Product Build();
}