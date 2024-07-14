using System.ComponentModel.DataAnnotations;

namespace PajoPhone.Models;

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public double Price { get; set; }
    public ICollection<FieldsValueViewModel> FieldsValues { get; set; } = new List<FieldsValueViewModel>();
    public ICollection<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    public int CategoryId { get; set; }
    public IFormFile? ImageFile { get; set; }
}
