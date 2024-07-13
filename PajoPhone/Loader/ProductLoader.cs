using Microsoft.EntityFrameworkCore;
using PajoPhone.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PajoPhone.Loader
{
    public class ProductLoader : IProductLoader
    {
        private readonly ApplicationDbContext _context;

        public ProductLoader(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> LoadProductAsync(int productId,
            bool includeCategory = false,
            bool includeFieldsValues = false)
        {
            var query = _context.Products.AsQueryable();

            if (includeCategory)
            {
                query = query.Include(p => p.Category);
            }

            if (includeFieldsValues)
            {
                query = query.Include(p => p.FieldsValues)
                    .ThenInclude(fv => fv.FieldKey);
            }
            var product = await query.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                throw new Exception("product not found!");
            }
            return product;
        }
    }
}