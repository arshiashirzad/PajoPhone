using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PajoPhone.Models;
using PajoPhone.Services.Factory;
using System.Web;
using AutoMapper;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.VisualBasic;

namespace PajoPhone.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<Product> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IProductFactory _productFactory;
        public ProductController(ApplicationDbContext context,IProductFactory productFactory)
        {
            _context = context;
            _productFactory = productFactory;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpGet]
        public JsonResult GetFieldKeys(int categoryId)
        {
            var keys = _context.FieldsKeys.Where(fk => fk.CategoryId == categoryId).ToList();
            return Json(keys);
        }
        // GET: Product/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            var viewModel = new ProductViewModel
            {
                Categories = await _context.Categories.ToListAsync(),
            };
            return View("Edit",viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> Create(ProductViewModel viewModel)
            => Edit(viewModel);

        // GET: Product/Edit/5
        public  IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product =  _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Color = product.Color,
                CategoryId = product.CategoryId,
                FieldsValues = product.FieldsValues
                    .Select(fk => new FieldsValueViewModel()
                    {
                        Id = fk.Id,
                        StringValue = fk.StringValue,
                        IntValue = fk.IntValue
                    }).ToList()
            };
            return View(productViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                Product product = await _productFactory.Save(productViewModel);
                return RedirectToAction("Details", new { id = product.Id });
            }
            return View();
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
