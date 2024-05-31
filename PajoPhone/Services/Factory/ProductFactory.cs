using PajoPhone.Models;

namespace PajoPhone.Services.Factory;

public class ProductFactory : IProductFactory
{
    private readonly ApplicationDbContext _context;
    private readonly IProductBuilder _productBuilder;
    public ProductFactory(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product> Save(ProductViewModel viewModel)
    {
        Product product;
        if (viewModel.Id == 0)
        {
            var productBuilder = _productBuilder;
             product = productBuilder.Build(viewModel);
            _context.Products.Add(product);
        }
        else
        {
            product = await _context.Products.FindAsync(viewModel.Id);
            if (product == null)
            {
                return null;
            }
            product.Name = viewModel.Name;
            product.CategoryId = viewModel.CategoryId;
            product.Fields = viewModel.Fields;
            _context.Products.Update(product);
        }
        await _context.SaveChangesAsync();
        return product;
    }
}