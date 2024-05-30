namespace PajoPhone.Models;

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Fields> Fields { get; set; } = new List<Fields>();
    
    public IFormFile ImageFile { get; set; }
}
