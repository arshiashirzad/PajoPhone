using Microsoft.AspNetCore.Mvc;
using PajoPhone.Models;

namespace PajoPhone.Repositories.Product;

public interface IProductRepository
{
    Task<Dictionary<string, List<FieldsValueViewModel>>> GetKeyValueItems(int categoryId);
    Task<List<FieldsValueViewModel>> GetKeyValueInputs(int categoryId ,int productId);
    Task<List<ProductViewModel>> FilterProducts(FilterViewModel filterViewModel);
}