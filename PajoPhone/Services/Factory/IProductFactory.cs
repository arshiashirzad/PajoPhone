using PajoPhone.Models;

namespace PajoPhone.Services.Factory;

public interface IProductFactory
{
     Task<Product> Save(ProductViewModel viewModel);
}