namespace PajoPhone.Models;

public class CategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentCategoryId { get; set; }
    public List<CategoryViewModel> ParentCategories { get; set; } = new List<CategoryViewModel>();
    public List<CategoryFieldViewModel> FieldsKeys { get; set; } = new List<CategoryFieldViewModel>();
}