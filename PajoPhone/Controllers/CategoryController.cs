using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PajoPhone.Models;

namespace PajoPhone.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private void PopulateParentCategories(CategoryViewModel viewModel)
        {
            viewModel.ParentCategories = _context.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }
        private List<object> GetCategoryTree(List<Category> categories, int? parentId)
        {
            return categories
                .Where(c => c.ParentCategoryId == parentId)
                .Select(c => new
                {
                    id = c.Id,
                    text = c.Name,
                    children = GetCategoryTree(categories, c.Id)
                }).ToList<object>();
        }
        public IActionResult GetCategoryTreeData()
        {
            var categories = _context.Categories.ToList();
            var categoryTreeData = GetCategoryTree(categories, null);
            return Json(categoryTreeData);
        }
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Category
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            var viewModel = new CategoryViewModel();
            PopulateParentCategories(viewModel); 
            return View("Edit",viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryViewModel categoryViewModel)
            => Edit(categoryViewModel);

        // GET: Categories/Edit/5
        public IActionResult Edit(int id)
        {
            var category = _context.Categories
                .Include(c => c.FieldsKeys)
                .FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            var viewModel = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId,
                FieldsKeys = category.FieldsKeys
                    .Select(fk => new CategoryFieldViewModel()
                    {
                        Id = fk.Id,
                        Name = fk.Key
                    }).ToList()
            };
            return View(viewModel);
        }
        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = _context.Categories
                    .Include(c => c.FieldsKeys)
                    .FirstOrDefault(c => c.Id == model.Id) ?? new Category();
                var modelFieldKeyIds = model.FieldsKeys.Select(fk => fk.Id).ToList();
                    category.Name = model.Name;
                    category.ParentCategoryId = model.ParentCategoryId;
                    foreach (var fv in model.FieldsKeys)
                    {
                        var currentFieldKey = category.FieldsKeys
                            .FirstOrDefault(f => f.Id == fv.Id);
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

                        if (model.Id != 0)
                        {
                            foreach (var existingFieldKey in category.FieldsKeys)
                            {
                                if (!modelFieldKeyIds.Contains(existingFieldKey.Id))
                                {
                                    existingFieldKey.DeletedAt = DateTime.Now;
                                }
                            }
                        }
                    } 
                    if (category.Id==0)
                    {
                        _context.Add(category);
                    }
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            return View(model);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
