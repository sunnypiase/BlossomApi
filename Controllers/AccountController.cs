using System.ComponentModel.DataAnnotations;
using BlossomApi.AttributeValidations;
using BlossomApi.DB;
using BlossomApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlossomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly BlossomContext _context;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            BlossomContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
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

            var siteUser = _context.SiteUsers.Add(new SiteUser
            {
                IdentityUserId = user.Id,
                Username = model.Username,
                Surname = model.Surname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            });

            await _context.SaveChangesAsync();
            await _context.ShoppingCarts.AddAsync(new ShoppingCart { SiteUserId = siteUser.Entity.UserId, CreatedDate = DateTime.UtcNow });
            await _context.SaveChangesAsync();

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok(new { Message = "Реєстрація успішна" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            _signInManager.AuthenticationScheme = IdentityConstants.ApplicationScheme;

            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                return Unauthorized(new { Message = "Невдала спроба входу." });
            }

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
        [PhoneNumber(ErrorMessage = "Неправильний формат номера телефону.")]
        public string PhoneNumber { get; set; }
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
