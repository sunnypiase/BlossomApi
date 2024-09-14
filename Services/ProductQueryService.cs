using BlossomApi.DB;
using BlossomApi.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IQueryable<Product>> ApplyFilterAndSortAsync(
            GetProductsByAdminFilterRequestDto request,
            Expression<Func<Product, bool>>? additionalFilter = null)
        {
            // Start with a base query
            var query = _context.Products.AsQueryable();

            // Apply additional filter if provided
            if (additionalFilter != null)
            {
                query = query.Where(additionalFilter);
            }

            // Search by name
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(p => p.Name.Contains(request.SearchTerm) || p.NameEng.Contains(request.SearchTerm));
            }

            // Filter by multiple categories
            if (request.CategoryIds != null && request.CategoryIds.Count > 0)
            {
                var allCategoryIds = new List<int>();

                foreach (var categoryId in request.CategoryIds)
                {
                    var rootCategory = await _context.Categories
                        .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

                    if (rootCategory != null)
                    {
                        var allCategories = new List<Category> { rootCategory };
                        allCategories.AddRange(await _categoryService.GetAllChildCategoriesAsync(rootCategory.CategoryId));
                        allCategoryIds.AddRange(allCategories.Select(c => c.CategoryId));
                    }
                }

                query = query.Where(p => p.Categories.Any(c => allCategoryIds.Contains(c.CategoryId)));
            }

            // Filter by brand
            if (request.BrandIds != null && request.BrandIds.Count > 0)
            {
                query = query.Where(p => request.BrandIds.Contains(p.Brand.BrandId));
            }

            // Filter by characteristics
            if (request.SelectedCharacteristics != null && request.SelectedCharacteristics.Count > 0)
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

            // Filter by IsNew
            if (request.IsNew.HasValue)
            {
                query = query.Where(p => p.IsNew == request.IsNew.Value);
            }

            // Filter by HasDiscount
            if (request.HasDiscount.HasValue)
            {
                query = query.Where(p => p.Discount > 0 == request.HasDiscount.Value);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortOption))
            {
                query = ApplySorting(query, request.SortOption);
            }

            return query;
        }

        private IQueryable<Product> ApplySorting(IQueryable<Product> query, string sortOption)
        {
            var sortParams = sortOption.Split('_');
            if (sortParams.Length == 2)
            {
                var sortBy = sortParams[0];
                var sortDirection = sortParams[1];
                bool sortDescending = sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = sortBy switch
                {
                    "name" => sortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                    "discount" => sortDescending ? query.OrderByDescending(p => p.Discount) : query.OrderBy(p => p.Discount),
                    "category" => sortDescending ? query.OrderByDescending(p => p.MainCategory.Name) : query.OrderBy(p => p.MainCategory.Name),
                    "price" => sortDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                    "stock" => sortDescending ? query.OrderByDescending(p => p.AvailableAmount) : query.OrderBy(p => p.AvailableAmount),
                    "popularity" => sortDescending ? query.OrderByDescending(p => p.NumberOfPurchases) : query.OrderBy(p => p.NumberOfPurchases),
                    "views" => sortDescending ? query.OrderByDescending(p => p.NumberOfViews) : query.OrderBy(p => p.NumberOfViews),
                    "rating" => sortDescending ? query.OrderByDescending(p => p.Rating) : query.OrderBy(p => p.Rating),
                    "reviews" => sortDescending ? query.OrderByDescending(p => p.NumberOfReviews) : query.OrderBy(p => p.NumberOfReviews),
                    _ => query
                };
            }

            return query;
        }
    }
}
