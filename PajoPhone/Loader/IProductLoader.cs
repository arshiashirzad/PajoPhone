namespace PajoPhone.Loader;
using PajoPhone.Models;
public interface IProductLoader
{
        Task<Product> LoadProductAsync(int productId,
            bool IncludeCategory = false,
            bool IncludeFieldsValues = false);
}