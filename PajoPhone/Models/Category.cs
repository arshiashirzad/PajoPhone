using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PajoPhone.Models;

public class Category
{      
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
}