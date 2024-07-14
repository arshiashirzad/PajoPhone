namespace PajoPhone.Loader;
using PajoPhone.Models;
public interface IProductLoader
{
        Product LoadSingleProduct(int productId,
            bool includeCategory = false,
            bool includeFieldsValues = false);

        IQueryable<Product> LoadProductList(
            bool includeCategory = false,
            bool includeFieldsValues = false);
}