using System.ComponentModel.DataAnnotations;

namespace PajoPhone.Models;

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public double Price { get; set; }
    public ICollection<FieldsKey> FieldsKey { get; set; } = new List<FieldsKey>();
    public ICollection<FieldsValue> FieldsValues { get; set; } = new List<FieldsValue>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public int CategoryId { get; set; }
    public IFormFile ImageFile { get; set; }
}
