using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PajoPhone.Models;

namespace PajoPhone.Services.Factory;

public class ProductFactory : IProductFactory
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public ProductFactory(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Product> Save(ProductViewModel viewModel)
    {
        var product = await _context.Products.FindAsync(viewModel.Id);
        product ??= new();
        if (viewModel.Id == 0)
        {
            var productBuilder = new ProductBuilder(_mapper,_context);
             product = productBuilder.Build(viewModel);
             productBuilder.Finalize(product);
        }
        else
        {
            var productEditor = new ProductEditor(_mapper);
            product = productEditor.Build(viewModel);
            productEditor.Finalize(product);
        }
        return product;
    }
}