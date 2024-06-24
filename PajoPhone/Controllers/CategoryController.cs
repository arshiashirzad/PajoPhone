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
                    FieldsKeys = categoryViewModel.FieldsKeys.Select(f => new FieldsKey
                    {
                        Key = f.Name
                    }).ToList()
                };
                if (Request.Form["NewFields[]"].Any())
                {
                    foreach (var fieldName in Request.Form["NewFields[]"])
                    {
                        category.FieldsKeys.Add(new FieldsKey { Key = fieldName });
                    }
                }
                
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

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
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
