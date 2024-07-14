using PajoPhone.Models;
using AutoMapper;
namespace PajoPhone;

public class ProductBuilder : IProductBuilder
{
    private readonly ApplicationDbContext _context ;
    private readonly IMapper _mapper;
    public ProductBuilder(IMapper mapper, ApplicationDbContext dbContext)
    {
        _mapper = mapper;
        _context = dbContext;
    }
    public void Finalize(Product product)
    {
        _context.Products.Add(product);
    }
    public Product Build(ProductViewModel viewModel , Product product)
    {
        _mapper.Map(viewModel, product);
        return product;
    }
}