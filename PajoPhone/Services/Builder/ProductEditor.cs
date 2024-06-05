using PajoPhone.Models;
using AutoMapper;
namespace PajoPhone;

public class ProductEditor: IProductBuilder
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public ProductEditor(IMapper mapper)
    {
        _mapper = mapper;
    }
    public IProductBuilder SetImage(IFormFile image)
    {
        
        return this;
    }
    public void Finalize(Product product)
    {
        _context.Products.Add(product);
    }
    public Product Build(ProductViewModel viewModel)
    {
        Product product = _mapper.Map<Product>(viewModel);
        if (viewModel.ImageFile.Length == 0)
        {
        }else
        {
            SetImage(viewModel.ImageFile);
        }
        return product;
    }
}