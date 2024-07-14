using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PajoPhone.Models;
using PajoPhone.Services.Factory;
using Microsoft.Extensions.Caching.Memory;
using PajoPhone.Loader;

namespace PajoPhone.Controllers
{
    [Route("/[action]")]
    [Route("/Product/[action]")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductFactory _productFactory;
        private readonly IProductLoader _productLoader;
        private readonly GooshiShopScraper _gooshiShopScraper;
        private readonly IMemoryCache _memoryCache;

        public ProductController(ApplicationDbContext context,
            IProductFactory productFactory ,
            IProductLoader productLoader,
            GooshiShopScraper gooshiShopScraper , IMemoryCache memoryCache)
        {
            _productLoader = productLoader;
            _context = context;
            _productFactory = productFactory;
            _gooshiShopScraper = gooshiShopScraper;
            _memoryCache = memoryCache;
        }
        public async Task<IActionResult> GetProductModal(int productId)
        {
            var product = await _productLoader.LoadProductAsync(productId,true  , true );
            return PartialView("_ProductModalPartial", product);
        }
        public async Task<IActionResult> GetSuggestions(string term)
        {
            var results = await _context.Products
                .Where(p => p.Name.StartsWith(term))
                .Select(p => p.Name )
                .Take(5)
                .ToListAsync();
            return Ok(results);
        }
        public async Task<IActionResult> GetProductCards(FilterViewModel filterViewModel)
        {
            if (filterViewModel == null)
            {
                filterViewModel = new FilterViewModel();
            }
            var query =  _context.Products
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
                    if (!string.IsNullOrEmpty(fieldsValue.StringValue))
                    {
                        query = query.Where(p => p.FieldsValues.Any(fv =>
                            fv.FieldKeyId == fieldsValue.KeyId &&
                            (fv.StringValue == fieldsValue.StringValue || fv.IntValue == fieldsValue.IntValue)));
                    }
                }
            }
            int pageSize = filterViewModel.PageNo * 10;
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
        public async Task<PartialViewResult> GetKeyValueInputs(int categoryId ,int productId)
        {
            var items= new List<FieldsValueViewModel>();
            var keys =await _context.FieldsKeys.Where(fk => fk.CategoryId == categoryId).ToListAsync();
            if (productId == 0)
            {
                items = keys.Select(x => new FieldsValueViewModel(x))
                    .ToList();
            }
            else
            {
                var query =await _context.FieldsValues.Where(x => x.ProductId == productId && keys.Contains(x.FieldKey!))
                    .ToListAsync();
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
        public async Task<IActionResult> GetPrice(string name)
        {
            var cacheKey = $"price_{name}";
            if (!_memoryCache.TryGetValue(cacheKey, out decimal price))
            {
                price = decimal.Parse(await _gooshiShopScraper.GetPriceAsync(name));
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(10)
                };

                _memoryCache.Set(cacheKey, price, cacheEntryOptions);
            }
            return Ok(price);
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
                await _context.SaveChangesAsync();
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
        private async Task<bool> ProductExists(int id)
        {
            return await _context.Products.AnyAsync(e => e.Id == id);
        }
    }
}