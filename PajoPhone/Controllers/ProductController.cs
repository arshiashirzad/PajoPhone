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
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic;

namespace PajoPhone.Controllers
{
    [Route("/[action]")]
    [Route("/Product/[action]")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductFactory _productFactory;

        public ProductController(ApplicationDbContext context, IProductFactory productFactory)
        {

            _context = context;
            _productFactory = productFactory;
        }

        public async Task<IActionResult> GetProductModal(int productId)
        {
            var product = await _context.Products
                .Include(x => x.FieldsValues)
                .ThenInclude(x => x.FieldKey)
                .Include(x=> x.Category)
                .SingleOrDefaultAsync(x => x.Id == productId);
            return PartialView("_ProductModalPartial", product);
        }
        
        public async Task<IActionResult> GetProductCards(FilterViewModel filterViewModel)
        {
            if (filterViewModel == null)
            {
                filterViewModel = new FilterViewModel();
            }
            var query = _context.Products
                .Include(p => p.FieldsValues)
                .ThenInclude(fv => fv.FieldKey)
                .Include(p => p.Category)
                .AsQueryable();
            if (filterViewModel.MinPrice!=0)
            {
                query = query.Where(p => p.Price >= filterViewModel.MinPrice);
            }
            if (filterViewModel.CategoryId!=0)
            {
                query = query.Where(p => p.CategoryId == filterViewModel.CategoryId);
            }
            if (!string.IsNullOrEmpty(filterViewModel.Term))
            {
                var searchTerms = filterViewModel.Term.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var term in searchTerms)
                {
                    query = query.Where(p => p.Name.Contains(term));
                }
            }
            if (filterViewModel.FieldsValueViewModels.Any())
            {
                foreach (var fieldsValue in filterViewModel.FieldsValueViewModels)
                {
                    query = query.Where(p => p.FieldsValues.Any(fv => 
                        fv.FieldKeyId == fieldsValue.KeyId && 
                        (fv.StringValue == fieldsValue.StringValue || fv.IntValue == fieldsValue.IntValue)));
                }
            }
            int pageSize = filterViewModel.PageNo *10;
            query = query.Take(pageSize);
            var products = await query.ToListAsync();
            var productViewModels = products.Select(product => new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Color = product.Color,
                Price = product.Price,
                Categories = new List<CategoryViewModel>
                {
                    new CategoryViewModel
                    {
                        Id = product.Category!.Id,
                        Name = product.Category.Name,
                        ParentCategoryId = product.Category.ParentCategoryId
                    }
                },
                CategoryId = product.CategoryId,
                FieldsValues = product.FieldsValues.Select(fv => new FieldsValueViewModel(fv)).ToList()
            }).ToList();
            
            return PartialView("_ProductCardsPartial", productViewModels);
        }
        // GET: Product
        [Route("/")]
        [Route("/Index")]
        [Route("/Product/Index")]
        public async Task<IActionResult> Index()
        {
            FilterViewModel filterViewModel = new FilterViewModel();
            return View(filterViewModel);
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
        public PartialViewResult GetKeyValueInputs(int categoryId ,int productId)
        {
            var items= new List<FieldsValueViewModel>();
            var keys = _context.FieldsKeys.Where(fk => fk.CategoryId == categoryId).ToList();
            if (productId == 0)
            {
                 items = keys.Select(x => new FieldsValueViewModel(x))
                    .ToList();
            }
            else
            {
                var query = _context.FieldsValues.Where(x => x.ProductId == productId && keys.Contains(x.FieldKey!))
                    .ToList();
                items = query.Select(x => new FieldsValueViewModel(x))
                    .ToList();
            }
            return PartialView("_KeyValueInputPartial",items);
        }
        public async Task<IActionResult> GetKeyValues(int categoryId)
        {
            var keys = await _context.FieldsKeys
                .Where(fk => fk.CategoryId == categoryId)
                .ToListAsync();
            Dictionary<string, List<FieldsValueViewModel>> items = new();
            foreach (var key in keys)
            {
                var values = await _context.FieldsValues
                    .Where(fv => fv.FieldKeyId == key.Id)
                    .Distinct()
                    .ToListAsync();
                var valueViewModels = values.Select(fv => new FieldsValueViewModel(fv)).ToList();
                items[key.Key!] = valueViewModels;
            }
            return Json(items);
        }
        // GET: Product/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            var categoryViewModels = categories.Select(category => new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            }).ToList();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
            var viewModel = new ProductViewModel
            {
                Categories = categoryViewModels
            };
            return View("Edit", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> Create(ProductViewModel viewModel)
            => Edit(viewModel);

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product =await _context.Products
                .Include(x=> x.FieldsValues)
                     .ThenInclude(x=> x.FieldKey)
                .SingleAsync(x=> x.Id == id);
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
                    .Select(fk => new FieldsValueViewModel(fk)).ToList()
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
                _context.SaveChanges();
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
            if (product == null)
            {
                throw new Exception("Product not found!");
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult GetImage(int id)
        {
            var product = _context.Products.Single(p => p.Id == id);
            byte[] fileContents = product.Image;
            string contentType = "image/JPEG";

            return File(fileContents, contentType);
        }
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
