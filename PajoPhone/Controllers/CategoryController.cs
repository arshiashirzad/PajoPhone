using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PajoPhone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PajoPhone.Repositories.Category;

namespace PajoPhone.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        // GET: Category
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new CategoryViewModel();
            viewModel.ParentCategories = await _categoryRepository.GetParentCategories();
            return View(nameof(Edit), viewModel);
        }
        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = viewModel.Name,
                    ParentCategoryId = viewModel.ParentCategoryId,
                };
                await _categoryRepository.AddAsync(category);
                return RedirectToAction(nameof(Index));
            }
            viewModel.ParentCategories = await _categoryRepository.GetParentCategories();
            return View(nameof(Edit),viewModel);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var viewModel = new CategoryViewModel(category.Id, category.Name , category.ParentCategoryId , null! , null!)
            {
                FieldsKeys = category.FieldsKeys.Select(fk => new CategoryFieldViewModel
                {
                    Id = fk.Id,
                    Name = fk.Key!
                }).ToList()
            };
            viewModel.ParentCategories = await _categoryRepository.GetParentCategories();
            return View(viewModel);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Update(viewModel);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }
        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  IActionResult DeleteConfirmed(int id)
        {
             _categoryRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        // GET: Category/GetCategoryTreeData
        public async Task<IActionResult> GetCategoryTreeData()
        {
            var categoryTreeData = await _categoryRepository.GetCategoryTreeAsync();
            return Json(categoryTreeData);
        }
    }
}
