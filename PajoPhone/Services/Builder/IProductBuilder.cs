using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PajoPhone.Models;
namespace PajoPhone;

public interface  IProductBuilder

{
    public Product Build(ProductViewModel viewModel, Product product);
    public IProductBuilder SetImage(IFormFile image);
    public void Finalize(Product product);
}