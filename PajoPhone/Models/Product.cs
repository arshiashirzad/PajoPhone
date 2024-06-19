namespace PajoPhone.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public byte[] Image { get; set; }
    public double Price { get; set; }
    public ICollection<Fields> Fields { get; set; }
    public Category Category { get; set; }
    public int CategoryId { get; set; }
    
    public virtual ICollection<FieldsValue> FieldsValues { get; set; } = new List<FieldsValue>();

}