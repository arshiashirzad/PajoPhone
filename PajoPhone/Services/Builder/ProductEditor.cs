using System.Net.Mime;
using PajoPhone.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PajoPhone;

public class ProductEditor: IProductBuilder
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public ProductEditor(IMapper mapper, ApplicationDbContext dbContext)
    {
        _mapper = mapper;
        _context = dbContext;
    }

    public void SetName(string name)
    {
        
    }
    public void SetDescription(string description)
    {
        
    }   
    public void SetPrice(double Price)
    {
        
    }
    public IProductBuilder SetImage(IFormFile image)
    {
        return this;
    }
    
    public void Finalize(Product product)
    {
    }
    public Product Build(ProductViewModel viewModel)
    {
        var product = _context.Products.Find(viewModel.Id);
        product = _mapper.Map<Product>(viewModel);
        return product;
    }
   
}