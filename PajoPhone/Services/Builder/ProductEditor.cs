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
    public IProductBuilder SetImage(IFormFile image)
    {
        return this;
    }
    
    public void Finalize(Product product)
    {
    }
    public Product Build(ProductViewModel viewModel, Product product)
    {
         _mapper.Map(viewModel, product);
        return product;
    }
}