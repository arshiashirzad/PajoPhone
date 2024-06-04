using PajoPhone.Models;

namespace PajoPhone.Services.Factory;

public class ProductFactory : IProductFactory
{
    private readonly ApplicationDbContext _context;
    public ProductFactory(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product> Save(ProductViewModel viewModel)
    {
        Product product = await _context.Products.FindAsync(viewModel.Id);
        product = (product == null) ? new Product() : product;
        if (viewModel.Id == 0)
        {
            var productBuilder = new ProductBuilder();
             product = productBuilder.Build(viewModel);
            _context.Products.Add(product);
        }
        else
        {
            var productEditor = new ProductEditor();
            product = productEditor.Build(viewModel);
            _context.Products.Update(product);
        }
        await _context.SaveChangesAsync();
        return product;
    }
}