using System.Linq;
using BlossomApi.DB;
using BlossomApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlossomApi.Services
{
    public class ProductAssociationService
    {
        private readonly BlossomContext _context;

        public ProductAssociationService(BlossomContext context)
        {
            _context = context;
        }

        public async Task UpdateProductAssociationsAsync<TEntity>(
            TEntity entity,
            List<int> newProductIds) where TEntity : class, IHasProducts
        {
            // Get the current product ids associated with the entity (blog or banner)
            var currentProductIds = entity.Products.Select(p => p.ProductId).ToList();

            // Find products to remove (those in the current collection but not in the new list)
            var productsToRemove = entity.Products.Where(p => !newProductIds.Contains(p.ProductId)).ToList();
            foreach (var productToRemove in productsToRemove)
            {
                entity.Products.Remove(productToRemove);
            }

            // Find products to add (those in the new list but not in the current collection)
            var productsToAddIds = newProductIds.Except(currentProductIds).ToList();
            if (productsToAddIds.Count > 0)
            {
                var productsToAdd = await _context.Products
                    .Where(p => productsToAddIds.Contains(p.ProductId))
                    .ToListAsync();

                foreach (var productToAdd in productsToAdd)
                {
                    entity.Products.Add(productToAdd);
                }
            }
        }
    }

// Define an interface that your entities can implement if they have products
    public interface IHasProducts
    {
        ICollection<Product> Products { get; set; }
    }
}
