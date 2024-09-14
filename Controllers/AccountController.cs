using System.ComponentModel.DataAnnotations;
using BlossomApi.AttributeValidations;
using BlossomApi.DB;
using BlossomApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Added for EF Core methods

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BlossomContext _context;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            BlossomContext context,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the email is already in use
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Користувач з такою електронною поштою вже існує.");
                return BadRequest(ModelState);
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return BadRequest(ModelState);
            }

            var siteUser = new SiteUser
            {
                IdentityUserId = user.Id,
                Username = model.Username,
                Surname = model.Surname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };
            _context.SiteUsers.Add(siteUser);
            await _context.SaveChangesAsync();

            // Link or create Cashback record based on phone number
            var cashback = await _context.Cashbacks
                .FirstOrDefaultAsync(c => c.PhoneNumber == model.PhoneNumber);

            if (cashback != null)
            {
                // If cashback exists and is not linked to any SiteUser, link it
                if (cashback.SiteUserId == null)
                {
                    cashback.SiteUserId = siteUser.UserId;
                    _context.Cashbacks.Update(cashback);
                    await _context.SaveChangesAsync();
                }
                else if (cashback.SiteUserId != siteUser.UserId)
                {
                    // If cashback is linked to another user, handle accordingly
                    ModelState.AddModelError(string.Empty, "Цей номер телефону вже пов'язаний з іншим обліковим записом.");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                // If no cashback record exists, create a new one
                cashback = new Cashback
                {
                    PhoneNumber = model.PhoneNumber,
                    Balance = 0,
                    SiteUserId = siteUser.UserId
                };
                _context.Cashbacks.Add(cashback);
                await _context.SaveChangesAsync();
            }

            // Create a new ShoppingCart for the user
            await _context.ShoppingCarts.AddAsync(new ShoppingCart { SiteUserId = siteUser.UserId, CreatedDate = DateTime.UtcNow });
            await _context.SaveChangesAsync();

            await _signInManager.SignInAsync(user, isPersistent: true);
            return Ok(new { Message = "Реєстрація успішна" });
        }

        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var adminSecret = Environment.GetEnvironmentVariable("ADMIN_SECRET");
            if (string.IsNullOrEmpty(adminSecret) || model.Secret != adminSecret)
            {
                return Unauthorized(new { Message = "Недійсний секретний ключ адміністратора." });
            }

            // Check if the email is already in use
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Користувач з такою електронною поштою вже існує.");
                return BadRequest(ModelState);
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return BadRequest(ModelState);
            }

            var siteUser = new SiteUser
            {
                IdentityUserId = user.Id,
                Username = model.Username,
                Surname = model.Surname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };
            _context.SiteUsers.Add(siteUser);
            await _context.SaveChangesAsync();

            // Link or create Cashback record based on phone number
            var cashback = await _context.Cashbacks
                .FirstOrDefaultAsync(c => c.PhoneNumber == model.PhoneNumber);

            if (cashback != null)
            {
                if (cashback.SiteUserId == null)
                {
                    cashback.SiteUserId = siteUser.UserId;
                    _context.Cashbacks.Update(cashback);
                    await _context.SaveChangesAsync();
                }
                else if (cashback.SiteUserId != siteUser.UserId)
                {
                    ModelState.AddModelError(string.Empty, "Цей номер телефону вже пов'язаний з іншим обліковим записом.");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                cashback = new Cashback
                {
                    PhoneNumber = model.PhoneNumber,
                    Balance = 0,
                    SiteUserId = siteUser.UserId
                };
                _context.Cashbacks.Add(cashback);
                await _context.SaveChangesAsync();
            }

            // Create a new ShoppingCart for the user
            await _context.ShoppingCarts.AddAsync(new ShoppingCart { SiteUserId = siteUser.UserId, CreatedDate = DateTime.UtcNow });
            await _context.SaveChangesAsync();

            // Assign Admin role
            var adminRole = "Admin";
            if (!await _roleManager.RoleExistsAsync(adminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(adminRole));
            }
            await _userManager.AddToRoleAsync(user, adminRole);

            await _signInManager.SignInAsync(user, isPersistent: true);
            return Ok(new { Message = "Реєстрація адміністратора успішна" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            _signInManager.AuthenticationScheme = IdentityConstants.ApplicationScheme;

            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, true, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                return Unauthorized(new { Message = "Невдала спроба входу." });
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("GetAuth")]
        public IActionResult GetAuth()
        {
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAdminAuth")]
        public IActionResult GetAdminAuth()
        {
            return Ok();
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Вихід успішний" });
        }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "Ім'я користувача є обов'язковим.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Прізвище є обов'язковим.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Електронна пошта є обов'язковою.")]
        [EmailAddress(ErrorMessage = "Неправильний формат електронної пошти.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль є обов'язковим.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Підтвердження паролю є обов'язковим.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Номер телефону є обов'язковим.")]
        [Phone(ErrorMessage = "Неправильний формат номера телефону.")]
        public string PhoneNumber { get; set; }

        public string? Secret { get; set; }
    }

    public class RegisterAdminModel : RegisterModel
    {
        [Required(ErrorMessage = "Секретний ключ адміністратора є обов'язковим.")]
        public new string Secret { get; set; }
    }

    public class LoginRequest
    {
        [Required(ErrorMessage = "Електронна пошта є обов'язковою.")]
        [EmailAddress(ErrorMessage = "Неправильний формат електронної пошти.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль є обов'язковим.")]
        public string Password { get; set; }
    }
}
