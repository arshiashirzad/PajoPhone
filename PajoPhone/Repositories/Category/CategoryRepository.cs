using Microsoft.EntityFrameworkCore;
using PajoPhone.Models;

namespace PajoPhone.Repositories.Category;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public bool CategoryExists(int id)
    {
        return _context.Categories.Any(e => e.Id == id);
    }
    public async Task<List<Models.Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }
    public async Task<Models.Category> GetByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            throw new ApplicationException($"Category with id {id} not found.");
        }
        return category;
    }

    public async Task AddAsync(Models.Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Models.Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<object>> GetCategoryTreeAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        return GetCategoryTree(categories, null);
    }

    private List<object> GetCategoryTree(List<Models.Category> categories, int? parentId)
    {
        return categories
            .Where(c => c.ParentCategoryId == parentId)
            .Select(c => new
            {
                id = c.Id,
                text = c.Name,
                children = GetCategoryTree(categories, c.Id)
            }).Cast<object>().ToList();
    }
}