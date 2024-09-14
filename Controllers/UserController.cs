using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlossomApi.DB;
using BlossomApi.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BlossomContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(BlossomContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/User
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userInfo = new
            {
                user.Username,
                user.Surname,
                user.Email,
                user.PhoneNumber,
                user.City,
                user.DepartmentNumber,
                FavoriteProducts = user.FavoriteProducts.Select(p => new
                {
                    p.ProductId,
                    p.Name,
                    p.Price,
                    p.Brand,
                    p.Images
                }).ToList()
            };

            return Ok(userInfo);
        }

        // PUT: api/User
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserDto updateUserDto)
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update the fields if they are provided in the DTO
            if (!string.IsNullOrEmpty(updateUserDto.Username))
            {
                user.Username = updateUserDto.Username;
            }

            if (!string.IsNullOrEmpty(updateUserDto.Surname))
            {
                user.Surname = updateUserDto.Surname;
            }

            if (!string.IsNullOrEmpty(updateUserDto.Email))
            {
                user.Email = updateUserDto.Email;
            }

            if (!string.IsNullOrEmpty(updateUserDto.PhoneNumber))
            {
                user.PhoneNumber = updateUserDto.PhoneNumber;
            }

            if (!string.IsNullOrEmpty(updateUserDto.City))
            {
                user.City = updateUserDto.City;
            }

            if (!string.IsNullOrEmpty(updateUserDto.DepartmentNumber))
            {
                user.DepartmentNumber = updateUserDto.DepartmentNumber;
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("User information updated successfully.");
        }

        private async Task<SiteUser?> GetCurrentUserAsync()
        {
            var identityUserId = _userManager.GetUserId(User);
            return await _context.SiteUsers
                .Include(u => u.FavoriteProducts)
                .Include(u => u.Cashback)
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);
        }
    }

    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? DepartmentNumber { get; set; }
    }
}
