using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PajoPhone.Models;
using PajoPhone.Services.Factory;
using Microsoft.Extensions.Caching.Memory;
using PajoPhone.Cache;
using PajoPhone.Loader;
using PajoPhone.Repositories.Product;

namespace PajoPhone.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductFactory _productFactory;
        private readonly IProductLoader _productLoader;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly PriceCacheManager _priceCacheManager;
        public ProductController(ApplicationDbContext context,
            IProductFactory productFactory ,
            IProductLoader productLoader,
            IMapper mapper,
            PriceCacheManager priceCacheManager,
            IProductRepository productRepository)
        {
            _productLoader = productLoader;
            _context = context;
            _productFactory = productFactory;
            _mapper = mapper;
            _priceCacheManager = priceCacheManager;
            _productRepository = productRepository;
        }
        public async Task<IActionResult> GetProductModal(int productId)
        {
            var product =  _productLoader.LoadSingleProduct(productId,true  , true );
            return PartialView("_ProductModalPartial", product);
        }
        public async Task<IActionResult> GetSearchSuggestions(string term)
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
            var productViewModels =await _productRepository.FilterProducts(filterViewModel);
            return PartialView("_ProductCardsPartial", productViewModels);
        }
        
        public async Task<PartialViewResult> GetKeyValueInputs(int categoryId ,int productId)
        {
            var items =await _productRepository.GetKeyValueInputs(categoryId, productId);
            return PartialView("_KeyValueInputPartial",items);
        }
        
        public async Task<IActionResult> GetKeyValues(int categoryId)
        {
            var items =await _productRepository.GetKeyValueItems(categoryId);
            return Json(items);
        }
        
        public async Task<IActionResult> GetPrice(string name)
        {
            var price =await _priceCacheManager.GetCachedPrice(name) ;
            return Ok(price);
        }
        
        // GET: Product
        [Route("/")]
        [Route("/Index")]
        [Route("/Product/Index")]
        public IActionResult Index()
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
            var product = _productLoader.LoadSingleProduct(id.Value, true);
            return View(product);
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
            
            var product = _productLoader.LoadSingleProduct(id.Value, true, true);
            ProductViewModel productViewModel = new ProductViewModel();
            _mapper.Map(product, productViewModel);
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
            var product = await _productLoader.LoadProductList(true)
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
            var product = _productLoader.LoadSingleProduct(id);
            byte[] fileContents = product.Image;
            string contentType = "image/JPEG";
            return File(fileContents, contentType);
        }
    }
}