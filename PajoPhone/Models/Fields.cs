using System.ComponentModel.DataAnnotations;

namespace PajoPhone.Models;

public class Fields
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }
}