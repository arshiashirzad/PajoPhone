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
        _context.Products.Update(product);
    }
    public Product Build(ProductViewModel viewModel)
    {
        Product product = _mapper.Map<Product>(viewModel);
        product.FieldsValues = (ICollection<FieldsValue>)viewModel.FieldsValues.Select(fk => new FieldsValue()
        {
            Id = fk.Id,
            IntValue = fk.IntValue,
            StringValue = fk.StringValue
        });
        if (viewModel.ImageFile.Length == 0)
        {
        }else
        {
            SetImage(viewModel.ImageFile);
        }
        return product;
    }
}