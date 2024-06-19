using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PajoPhone.Models;

public class Category
{      
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        //using virtual for lazy loading
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> ChildCategories { get; set; } = new List<Category>();
        public virtual ICollection<Fields> Fields { get; set; } = new List<Fields>();
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}