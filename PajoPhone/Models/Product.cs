using System.ComponentModel.DataAnnotations;

namespace PajoPhone.Models;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; } 
    public required string Description { get; set; } 
    public required string Color { get; set; } 
    public required byte[] Image { get; set; }
    public double Price { get; set; }
    public Category? Category { get; set; }
    public int CategoryId { get; set; }
    
    public  List<FieldsValue> FieldsValues { get; set; } = [];

}