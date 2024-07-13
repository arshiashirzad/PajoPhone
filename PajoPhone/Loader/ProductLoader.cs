using Microsoft.EntityFrameworkCore;
using PajoPhone.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PajoPhone.Loaders
{
    public class ProductLoader
    {
        private readonly ApplicationDbContext _context;

        public ProductLoader(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> SingleAsync(
            bool includeCategory = false,
            bool includeFieldsValues = false)
        {
            var model =_context.Products.AsQueryable();
            if (includeCategory)
            {
                model = model.Include(p => p.Category);
            }
            if (includeFieldsValues)
            {
                model = model.Include(p => p.FieldsValues)
                    .ThenInclude(fk => fk.FieldKey);
            }
                return model;
        }
    }
}