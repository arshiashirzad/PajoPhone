using System.ComponentModel.DataAnnotations;

namespace PajoPhone.Models;

public class FieldsKey
{
    [Key]
    public int Id { get; set; }
    public string Key { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}