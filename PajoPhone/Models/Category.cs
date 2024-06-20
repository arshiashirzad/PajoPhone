using System.ComponentModel.DataAnnotations.Schema;

namespace PajoPhone.Models;

public class Category
{
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; }
        public ICollection<Category> ChildCategories { get; set; } = new List<Category>();
        public ICollection<FieldsKey> FieldsKeys { get; set; } = new List<FieldsKey>(); 
        public ICollection<Product> Products { get; set; } = new List<Product>();  
}