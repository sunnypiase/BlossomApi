using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlossomApi.DB;
using BlossomApi.Models;
using Microsoft.EntityFrameworkCore;
using BlossomApi.Dtos;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BlossomApi.Dtos.Reviews;

namespace BlossomApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReviewsController(BlossomContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Reviews/Product/{productId}
        [HttpGet("Product/{productId}")]
        public async Task<IActionResult> GetReviewsByProduct(int productId, int page = 0, int pageSize = 10)
        {
            var query = _context.Reviews
                .Include(r => r.SiteUser)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.Date);

            var totalReviews = await query.CountAsync();
            var totalPages = (int)System.Math.Ceiling(totalReviews / (double)pageSize);

            var reviews = await query
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var reviewDtos = reviews.Select(r => new ReviewDto
            {
                ReviewId = r.ReviewId,
                ReviewText = r.ReviewText,
                Rating = r.Rating,
                Date = r.Date,
                ProductId = r.ProductId,
                SiteUserId = r.SiteUserId,
                Username = r.SiteUser.Username
            }).ToList();

            return Ok(new
            {
                Reviews = reviewDtos,
                Pagination = new
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    TotalReviews = totalReviews
                }
            });
        }

        // POST: api/Reviews
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDto reviewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var siteUser = await GetCurrentUserAsync();
            if (siteUser == null)
            {
                return Unauthorized();
            }

            var product = await _context.Products.FindAsync(reviewDto.ProductId);
            if (product == null)
            {
                return NotFound("Товар не знайдено.");
            }

            // Перевірка, чи вже існує відгук від цього користувача на цей товар
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.ProductId == reviewDto.ProductId && r.SiteUserId == siteUser.UserId);

            if (existingReview != null)
            {
                return BadRequest("Ви вже залишали відгук на цей товар.");
            }

            var review = new Review
            {
                ReviewText = reviewDto.ReviewText,
                Rating = reviewDto.Rating,
                Date = System.DateTime.UtcNow,
                ProductId = reviewDto.ProductId,
                SiteUserId = siteUser.UserId
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Оновлення рейтингу продукту
            await UpdateProductRating(reviewDto.ProductId);

            return Ok(new { Message = "Відгук успішно додано." });
        }

        // DELETE: api/Reviews/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var siteUser = await GetCurrentUserAsync();
            if (siteUser == null)
            {
                return Unauthorized();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound("Відгук не знайдено.");
            }

            // Тільки автор відгуку або адміністратор може видалити відгук
            if (review.SiteUserId != siteUser.UserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            // Оновлення рейтингу продукту
            await UpdateProductRating(review.ProductId);

            return Ok(new { Message = "Відгук успішно видалено." });
        }

        // Адміністративні методи
        public class SearchReviewsQueryDto
        {
            public string? ProductName { get; set; }
            public string? Username { get; set; }
            public string? SortBy { get; set; }  // Nullable now
            public int Page { get; set; } = 0;
            public int PageSize { get; set; } = 10;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Admin/Search")]
        public async Task<IActionResult> SearchReviews([FromBody] SearchReviewsQueryDto query)
        {
            var reviewsQuery = _context.Reviews
                .Include(r => r.SiteUser)
                .Include(r => r.Product)
                .AsQueryable();

            // Case-insensitive search for product name
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                var lowerProductName = query.ProductName.ToLower();
                reviewsQuery = reviewsQuery.Where(r => r.Product.Name.ToLower().Contains(lowerProductName));
            }

            // Case-insensitive search for username
            if (!string.IsNullOrEmpty(query.Username))
            {
                var lowerUsername = query.Username.ToLower();
                reviewsQuery = reviewsQuery.Where(r => r.SiteUser.Username.ToLower().Contains(lowerUsername));
            }

            // Handle nullable SortBy with default value
            var sortBy = query.SortBy?.ToLower() ?? "date_desc";

            // Sorting logic
            reviewsQuery = sortBy switch
            {
                "commenttext_asc" => reviewsQuery.OrderBy(r => r.ReviewText),
                "commenttext_desc" => reviewsQuery.OrderByDescending(r => r.ReviewText),
                "date_asc" => reviewsQuery.OrderBy(r => r.Date),
                "date_desc" => reviewsQuery.OrderByDescending(r => r.Date),
                "username_asc" => reviewsQuery.OrderBy(r => r.SiteUser.Username),
                "username_desc" => reviewsQuery.OrderByDescending(r => r.SiteUser.Username),
                "rating_asc" => reviewsQuery.OrderBy(r => r.Rating),
                "rating_desc" => reviewsQuery.OrderByDescending(r => r.Rating),
                "productname_asc" => reviewsQuery.OrderBy(r => r.Product.Name),
                "productname_desc" => reviewsQuery.OrderByDescending(r => r.Product.Name),
                _ => reviewsQuery.OrderByDescending(r => r.Date), // Default sorting
            };

            var totalReviews = await reviewsQuery.CountAsync();
            var totalPages = (int)System.Math.Ceiling(totalReviews / (double)query.PageSize);

            var reviews = await reviewsQuery
                .Skip(query.Page * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var reviewDtos = reviews.Select(r => new ReviewDto
            {
                ReviewId = r.ReviewId,
                ReviewText = r.ReviewText,
                Rating = r.Rating,
                Date = r.Date,
                ProductId = r.ProductId,
                ProductName = r.Product.Name,
                SiteUserId = r.SiteUserId,
                Username = r.SiteUser.Username
            }).ToList();

            return Ok(new
            {
                Reviews = reviewDtos,
                Pagination = new
                {
                    CurrentPage = query.Page,
                    TotalPages = totalPages,
                    PageSize = query.PageSize,
                    TotalReviews = totalReviews
                }
            });
        }


        // DELETE: api/Reviews/Admin/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("Admin/{id}")]
        public async Task<IActionResult> DeleteReviewByAdmin(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound("Відгук не знайдено.");
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            // Оновлення рейтингу продукту
            await UpdateProductRating(review.ProductId);

            return Ok(new { Message = "Відгук успішно видалено адміністратором." });
        }

        // DELETE: api/Reviews/Admin/DeleteByUser/{userId}
        [Authorize(Roles = "Admin")]
        [HttpDelete("Admin/DeleteByUser/{userId}")]
        public async Task<IActionResult> DeleteReviewsByUser(int userId)
        {
            var reviews = _context.Reviews.Where(r => r.SiteUserId == userId);

            if (!reviews.Any())
            {
                return NotFound("Відгуки для заданого користувача не знайдено.");
            }

            var productIds = reviews.Select(r => r.ProductId).Distinct().ToList();

            _context.Reviews.RemoveRange(reviews);
            await _context.SaveChangesAsync();

            // Оновлення рейтингу продуктів
            foreach (var pid in productIds)
            {
                await UpdateProductRating(pid);
            }

            return Ok(new { Message = "Всі відгуки користувача успішно видалено." });
        }

        // POST: api/Reviews/Admin/DeleteMultiple
        [Authorize(Roles = "Admin")]
        [HttpPost("Admin/DeleteMultiple")]
        public async Task<IActionResult> DeleteMultipleReviews([FromBody] List<int> reviewIds)
        {
            var reviews = _context.Reviews.Where(r => reviewIds.Contains(r.ReviewId));

            if (!reviews.Any())
            {
                return NotFound("Відгуки з заданими ID не знайдено.");
            }

            var productIds = reviews.Select(r => r.ProductId).Distinct().ToList();

            _context.Reviews.RemoveRange(reviews);
            await _context.SaveChangesAsync();

            // Оновлення рейтингу продуктів
            foreach (var pid in productIds)
            {
                await UpdateProductRating(pid);
            }

            return Ok(new { Message = "Вибрані відгуки успішно видалено." });
        }

        // Допоміжні методи

        private async Task<SiteUser> GetCurrentUserAsync()
        {
            var identityUserId = _userManager.GetUserId(User);
            return await _context.SiteUsers
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);
        }

        private async Task UpdateProductRating(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product != null)
            {
                if (product.Reviews.Any())
                {
                    product.Rating = product.Reviews.Average(r => r.Rating);
                    product.NumberOfReviews = product.Reviews.Count();
                }
                else
                {
                    product.Rating = 0;
                    product.NumberOfReviews = 0;
                }

                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
