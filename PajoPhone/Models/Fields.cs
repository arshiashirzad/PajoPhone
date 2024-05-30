using System.ComponentModel.DataAnnotations;

namespace PajoPhone.Models;

public class Fields
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Information { get; set; }
    public Product Product { get; set; }
    public int ProductId { get; set; }
}