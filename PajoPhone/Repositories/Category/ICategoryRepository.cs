namespace PajoPhone.Repositories.Category;
using PajoPhone.Models;
public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category> GetByIdAsync(int id);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
    bool CategoryExists(int id);
    Task<List<CategoryViewModel>> GetParentCategories();
    Task<List<object>> GetCategoryTreeAsync();
    List<object> GetCategoryTree(List<Category> categories, int? parentId);
    Category Update(CategoryViewModel category);
}
