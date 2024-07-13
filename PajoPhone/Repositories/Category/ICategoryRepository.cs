namespace PajoPhone.Repositories.Category;
using PajoPhone.Models;
public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category> GetByIdAsync(int id);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
    Task<List<object>> GetCategoryTreeAsync();
    
    bool CategoryExists(int id);
}
