using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PajoPhone.Loader;
using PajoPhone.Models;
namespace PajoPhone.Repositories.Product;

public class ProductRepository : IProductRepository
{
    private readonly IProductLoader _productLoader;
    private readonly ApplicationDbContext _context;
    public ProductRepository(IProductLoader productLoader, ApplicationDbContext context)
    {
        _productLoader=productLoader;
        _context = context;
    }
    public async Task<Dictionary<string, List<FieldsValueViewModel>>> GetKeyValueItems(int categoryId)
    {
        var keys = await _context.FieldsKeys
            .Where(fk => fk.CategoryId == categoryId)
            .ToListAsync();
        Dictionary<string, List<FieldsValueViewModel>> items = new();
        foreach (var key in keys)
        {
            var values = await _context.FieldsValues
                .Where(fv => fv.FieldKeyId == key.Id)
                .Distinct()
                .ToListAsync();
            var valueViewModels = values.Select(fv => new FieldsValueViewModel(fv)).ToList();
            items[key.Key!] = valueViewModels;
        }
        return items;
    }

    public async Task<List<FieldsValueViewModel>> GetKeyValueInputs(int categoryId, int productId)
    {
        List<FieldsValueViewModel> items= new List<FieldsValueViewModel>();
        var keys =await _context.FieldsKeys.Where(fk => fk.CategoryId == categoryId).ToListAsync();
        if (productId == 0)
        {
            items = keys.Select(x => new FieldsValueViewModel(x))
                .ToList();
        }
        else
        {
            var query =await _context.FieldsValues.Where(x => x.ProductId == productId && keys.Contains(x.FieldKey!))
                .ToListAsync();
            items = query.Select(x => new FieldsValueViewModel(x))
                .ToList();
        }
        return items;
    }
    public async Task<List<ProductViewModel>> FilterProducts(FilterViewModel filterViewModel)
    {
            var query = _productLoader.LoadProductList(true, true);
            if (filterViewModel.MinPrice!=0)
            {
                query = query.Where(p => p.Price >= filterViewModel.MinPrice);
            }
            if (filterViewModel.CategoryId!=0)
            {
                query = query.Where(p => p.CategoryId == filterViewModel.CategoryId);
            }
            if (!string.IsNullOrEmpty(filterViewModel.Term))
            {
                var searchTerms = filterViewModel.Term.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var term in searchTerms)
                {
                    query = query.Where(p => p.Name.Contains(term));
                }
            }
            if (filterViewModel.FieldsValueViewModels.Any())
            {
                foreach (var fieldsValue in filterViewModel.FieldsValueViewModels)
                {
                    if (!string.IsNullOrEmpty(fieldsValue.StringValue))
                    {
                        query = query.Where(p => p.FieldsValues.Any(fv =>
                            fv.FieldKeyId == fieldsValue.KeyId &&
                            (fv.StringValue == fieldsValue.StringValue || fv.IntValue == fieldsValue.IntValue)));
                    }
                }
            }
            int pageSize = filterViewModel.PageNo * 10;
            query = query.Take(pageSize);
            var products = await query.ToListAsync();
            var productViewModels = products.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Color = product.Color,
                Price = product.Price,
                Categories = new List<CategoryViewModel>(),
                CategoryId = product.CategoryId,
                FieldsValues = product.FieldsValues.Select(fv => new FieldsValueViewModel(fv)).ToList()
            }).ToList();
            return productViewModels;
    }
}