using System.Drawing;
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
    public Product Build(ProductViewModel viewModel, Product product)
    {
        byte[] tempImg = product.Image;
         _mapper.Map(viewModel, product);
         if (viewModel.ImageFile == null)
         {
             product.Image = tempImg; 
         }
        return product;
    }
}