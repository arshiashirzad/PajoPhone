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
            return View(viewModel);        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    Name = categoryViewModel.Name,
                    ParentCategoryId = categoryViewModel.ParentCategoryId,
                };
                category.FieldsKeys = categoryViewModel.FieldsKeys.Select(f => new FieldsKey
                {
                    Key = f.Name,
                    CategoryId = category.Id
                }).ToList();
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            categoryViewModel.ParentCategories = _context.Categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
            return View(categoryViewModel);
        }

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
                    .FirstOrDefault(c => c.Id == model.Id);

                if (category == null)
                {
                    return NotFound();
                }
                category.Name = model.Name;
                category.ParentCategoryId = model.ParentCategoryId;
                _context.FieldsKeys.RemoveRange(category.FieldsKeys);
                _context.SaveChanges();

                category.FieldsKeys.Clear();
                foreach (var field in model.FieldsKeys)
                {
                    category.FieldsKeys.Add(new FieldsKey
                    {
                        Key = field.Name,
                        CategoryId = category.Id
                    });
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
