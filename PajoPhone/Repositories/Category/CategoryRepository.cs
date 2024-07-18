using Bogus.DataSets;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<List<CategoryViewModel>> GetParentCategories()
    {
        var categories = await GetAllAsync();
        return categories.Select(c => new CategoryViewModel
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();
    }
    public async Task<bool> CategoryExists(int id)
    {
        return await _context.Categories.AnyAsync(e => e.Id == id);
    }
    public async Task<List<Models.Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }
    public async Task<Models.Category> GetByIdAsync(int id)
    {
        var category = await _context.Categories.Include(fk => fk.FieldsKeys)
            .FirstOrDefaultAsync(c=> c.Id == id);
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
    public void DeleteAsync(int id)
    {
        var category =  _context.Categories.SingleOrDefault(c => c.Id == id);
        if (category != null)
        {
            category.DeletedAt =DateTime.UtcNow;
        }
        _context.SaveChanges();
    }
    public async Task<List<CategoryViewModel>> GetCategoryTreeAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        var categoryTreeData = GetCategoryTree(categories, null);
        return categoryTreeData;
    }
    private List<CategoryViewModel> GetCategoryTree(List<Models.Category> categories, int? parentId)
    {
        return categories
            .Where(c => c.ParentCategoryId == parentId)
            .Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ParentCategoryId = c.ParentCategoryId,
                ParentCategories = GetCategoryTree(categories, c.Id),
                FieldsKeys = new List<CategoryFieldViewModel>() 
            })
            .ToList();
    }

    public Models.Category Update(CategoryViewModel categoryViewModel)
    {
        var category = _context.Categories
            .Include(c => c.FieldsKeys)
            .FirstOrDefault(c => c.Id == categoryViewModel.Id) ?? new Models.Category()
        {
            Name = string.Empty
        };
        var modelFieldKeyIds = categoryViewModel.FieldsKeys.Select(fk => fk.Id).ToList();
        category.Name = categoryViewModel.Name;
        category.ParentCategoryId = categoryViewModel.ParentCategoryId;
        foreach (var fv in categoryViewModel.FieldsKeys)
        {
            var currentFieldKey = category.FieldsKeys
                .FirstOrDefault(f => f.Id == fv.Id);
            if (categoryViewModel.Id != 0)
            {
                foreach (var existingFieldKey in category.FieldsKeys)
                {
                    if (!modelFieldKeyIds.Contains(existingFieldKey.Id))
                    {
                        existingFieldKey.DeletedAt = DateTime.Now;
                    }
                }
            }
            if (currentFieldKey != null)
            {
                currentFieldKey.Key = fv.Name;
                currentFieldKey.DeletedAt = null;
            }
            else
            {                
                category.FieldsKeys.Add(new FieldsKey()
                {
                    Key = fv.Name,
                    DeletedAt = null
                });
            }
        } 
        if (category.Id==0)
        {
            _context.Add(category);
        }
        _context.SaveChanges();
        return category;
    }
}