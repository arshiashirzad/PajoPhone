namespace PajoPhone.Loader;
using PajoPhone.Models;
public interface IProductLoader
{
        Task<Product> SingleAsync(int id,
            bool IncludeCategory = false,
            bool IncludeFieldsValues = false);
}