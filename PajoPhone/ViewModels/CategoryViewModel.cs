namespace PajoPhone.Models;

public class CategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentCategoryId { get; set; }
    public List<CategoryViewModel> ParentCategories { get; set; } = new List<CategoryViewModel>();
    public List<CategoryFieldViewModel> FieldsKeys { get; set; } = new List<CategoryFieldViewModel>();
    public string text => Name;
    public List<CategoryViewModel> children => ParentCategories;
    public CategoryViewModel()
    {
    }
    public CategoryViewModel(int id, string name, int? parentCategoryId, List<CategoryViewModel> parentCategories, List<CategoryFieldViewModel> fieldsKeys)
    {
        Id = id;
        Name = name;
        ParentCategoryId = parentCategoryId;
        ParentCategories = parentCategories ?? new List<CategoryViewModel>();
        FieldsKeys = fieldsKeys ?? new List<CategoryFieldViewModel>();
    }
}
