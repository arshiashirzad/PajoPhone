using System.ComponentModel.DataAnnotations.Schema;

namespace PajoPhone.Models;

public class Category
{
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public List<Category> ChildCategories { get; set; } = [];
        public List<FieldsKey> FieldsKeys { get; set; } = [];
        public List<Product> Products { get; set; } = []; 
}