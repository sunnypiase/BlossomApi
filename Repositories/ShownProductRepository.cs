using BlossomApi.DB;
using BlossomApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Repositories
{
    public class ShownProductRepository : IShownProductRepository
    {
        private readonly BlossomContext _context;

        public ShownProductRepository(BlossomContext context)
        {
            _context = context;
        }

        public IQueryable<Product> GetProducts(bool isShown = true)
        {
            return _context.Products.Where(p => p.IsShown == isShown);
        }
    }
}
