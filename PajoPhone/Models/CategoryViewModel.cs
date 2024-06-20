namespace PajoPhone.Models;

public class CategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentCategoryId { get; set; }
    public List<CategoryViewModel> ParentCategories { get; set; }
    public List<FieldsKey> FieldsKeys { get; set; }
    public List<CategoryFieldKeyViewModel> Fields { get; set; } = new List<CategoryFieldKeyViewModel>();

}