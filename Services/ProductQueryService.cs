using BlossomApi.Models;
using BlossomApi.Dtos;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using BlossomApi.DB;

namespace BlossomApi.Services
{
    public class ProductQueryService
    {
        private readonly BlossomContext _context;
        private readonly CategoryService _categoryService;

        public ProductQueryService(BlossomContext context, CategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        public async Task<IQueryable<Product>> ApplyFilterAndSortAsync(GetProductsByAdminFilterRequestDto request)
        {
            var query = _context.Products.AsQueryable();

            // Search by name
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(p => p.Name.Contains(request.SearchTerm) || p.NameEng.Contains(request.SearchTerm));
            }

            // Filter by category and characteristics
            if (request.CategoryId != null)
            {
                var rootCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId == request.CategoryId);

                if (rootCategory != null)
                {
                    var allCategories = new List<Category> { rootCategory };
                    allCategories.AddRange(await _categoryService.GetAllChildCategoriesAsync(rootCategory.CategoryId));

                    var categoryIds = allCategories.Select(c => c.CategoryId).ToList();
                    query = query.Where(p => p.Categories.Any(c => categoryIds.Contains(c.CategoryId)));
                }
            }

            if (request.SelectedCharacteristics != null && request.SelectedCharacteristics.Count != 0)
            {
                var characteristicIds = request.SelectedCharacteristics;
                Expression<Func<Product, bool>> predicate = p => false;

                predicate = characteristicIds
                    .Aggregate(predicate, (current, temp) => current.Or(p => p.Characteristics.Any(c => c.CharacteristicId == temp)));

                query = query.Where(predicate);
            }

            // Filter by price
            if (request.MinPrice.HasValue && request.MinPrice.Value > 0)
            {
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue && request.MaxPrice.Value > 0)
            {
                query = query.Where(p => p.Price <= request.MaxPrice.Value);
            }

            // Filter by IsShown
            if (request.IsShown.HasValue)
            {
                query = query.Where(p => p.IsShown == request.IsShown.Value);
            }

            // Filter by IsHit
            if (request.IsHit.HasValue)
            {
                query = query.Where(p => p.IsHit == request.IsHit.Value);
            }

            // Filter by HasDiscount
            if (request.HasDiscount.HasValue)
            {
                query = query.Where(p => p.Discount > 0 == request.HasDiscount.Value);
            }

            // Sorting logic based on SortOption
            if (!string.IsNullOrEmpty(request.SortOption))
            {
                var sortOption = request.SortOption.Split('_');
                if (sortOption.Length == 2)
                {
                    var sortBy = sortOption[0];
                    var sortDirection = sortOption[1];
                    bool sortDescending = sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

                    query = sortBy switch
                    {
                        "name" => sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                        "discount" => sortDescending ? query.OrderByDescending(p => p.Discount) : query.OrderBy(p => p.Discount),
                        "category" => sortDescending ? query.OrderByDescending(p => p.Categories.FirstOrDefault().Name) : query.OrderBy(p => p.Categories.FirstOrDefault().Name),
                        "price" => sortDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                        "stock" => sortDescending ? query.OrderByDescending(p => p.AvailableAmount) : query.OrderBy(p => p.AvailableAmount),
                        "popularity" => sortDescending ? query.OrderByDescending(p => p.NumberOfPurchases) : query.OrderBy(p => p.NumberOfPurchases),
                        "views" => sortDescending ? query.OrderByDescending(p => p.NumberOfViews) : query.OrderBy(p => p.NumberOfViews),
                        "rating" => sortDescending ? query.OrderByDescending(p => p.Rating) : query.OrderBy(p => p.Rating),
                        "reviews" => sortDescending ? query.OrderByDescending(p => p.NumberOfReviews) : query.OrderBy(p => p.NumberOfReviews),
                        _ => query
                    };
                }
            }

            return query;
        }
    }
}
